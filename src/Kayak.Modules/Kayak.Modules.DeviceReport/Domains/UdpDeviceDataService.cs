using DotNetty.Buffers;
using Kayak.IModuleServices.DeviceReport;
using Surging.Core.CPlatform.Ioc;
using Surging.Core.CPlatform.Protocol;
using Surging.Core.DeviceGateway.Runtime.Core;
using Surging.Core.DeviceGateway.Runtime.Device.Implementation;
using Surging.Core.DeviceGateway.Runtime.Device;
using Surging.Core.DeviceGateway.Runtime.session;
using Surging.Core.DeviceGateway.Runtime.Session;
using Surging.Core.Protocol.Tcp.Runtime;
using Surging.Core.Protocol.Udp.Runtime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reactive.Linq;
using Surging.Core.DeviceGateway.Runtime.Session.Implementation;
using Surging.Core.Protocol.Udp.Runtime.Implementation;
using Surging.Core.CPlatform.Network;
using Surging.Core.DeviceGateway.Runtime.Device.Message;
using Surging.Core.DeviceGateway.Runtime.Device.MessageCodec;
using Surging.Core.DeviceGateway.Runtime.device;
using Kayak.IModuleServices.DeviceManage;
using Kayak.IModuleServices.DeviceManage.Models;
using Surging.Core.Protocol.Udp.Messages;
using Kayak.Core.Common.Response;
using Kayak.IModuleServices.DeviceManage.Queries;
using Kayak.IModuleServices.System.Models;
using Kayak.IModuleServices.System;
using Surging.Core.DeviceGateway.Runtime.Core.Implementation;
using Surging.Core.DeviceGateway.Runtime.Core.Metadata;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;
using DotNetty.Common.Utilities;
using Surging.Core.DeviceGateway.Runtime.Device.Message.Property;
using Surging.Core.CPlatform.Messages;
using Surging.Core.DeviceGateway.Runtime.Device.Implementation.Http;
using Surging.Core.DeviceGateway.Runtime.Device.Message.Event;

namespace Kayak.Modules.DeviceReport.Domains
{
    internal class UdpDeviceDataService : UdpBehavior, IUdpDeviceDataService
    {
        private readonly IDeviceProvider _deviceProvider; 
        private IDeviceGateway _deviceGateway;
        private ProtocolSupport _protocolSupport;
        private IDeviceSessionManager _deviceSessionManager;
        private DeviceGatewayProperties _deviceGatewayProperties; 
        private string _productCode;
        private readonly IDeviceRegistry _registry;
        private string _deviceId;

        private static readonly AttributeKey<Tuple<string,string>> _deviceProductKey = AttributeKey<Tuple<string, string>>.ValueOf(typeof(UdpDeviceDataService), nameof(Tuple<string, string>));
 
        public UdpDeviceDataService(IDeviceProvider deviceProvider, IDeviceRegistry registry, IDeviceGatewayManage deviceGatewayManage, IProtocolSupports protocolSupports, IDeviceSessionManager deviceSessionManager)
        {
            _deviceProvider = deviceProvider;

            _deviceSessionManager = deviceSessionManager;
            _registry = registry;
            NetworkId.Subscribe(networkId =>
            {
                if (networkId != null)
                {
                    _deviceGatewayProperties = deviceGatewayManage.GetGatewayProperties().Where(p => p.ChannelId == networkId).FirstOrDefault();
                    if (_deviceGatewayProperties != null)
                    {
                        protocolSupports.GetProtocol(_deviceGatewayProperties.Protocol).Subscribe(async (protocolSupport) =>
                        {
                            _protocolSupport = (ProtocolSupport)protocolSupport;
                            _deviceGateway = await deviceGatewayManage.GetGateway(_deviceGatewayProperties.Id).FirstOrDefaultAsync();
                        });
                    }
                }
            });
        }

        public Task<bool> ChangeDeviceStage(string deviceId)
        {
            throw new NotImplementedException();
        }

        public override async Task Dispatch(IEnumerable<byte> bytes)
        {
            try
            {
                List<string> result = new List<string>();
                var complexProtocolSupport = _protocolSupport as ComplexProtocolSupport;
                var deviceProduct= Sender.Get(_deviceProductKey);
                _deviceId = deviceProduct?.Item1;
                _productCode = deviceProduct?.Item2;
                var messageCodecSupport = await complexProtocolSupport?.GetMessageCodecSupport(_deviceGatewayProperties.Transport);
                if (messageCodecSupport != null)
                {
                    var deviceSession= _deviceId==null ?default: await _deviceSessionManager.GetSession(_deviceId);
                     messageCodecSupport.Decode(new MessageDecodeContext(_deviceId, new UdpMessage(Unpooled.WrappedBuffer(bytes.ToArray())), deviceSession, _registry)).Subscribe(async message =>
                    {
                        try
                        {
                            if (message != null)
                            {
                                if (message.MessageType == MessageType.ONLINE)
                                {
                                    await HandleOnlineMessage(message);
                                }
                                if (message.MessageType == MessageType.READ_PROPERTY)
                                {
                                    await HandleReadPropertyMessage(message);
                                    await ReplyMessage(messageCodecSupport, message);
                                }
                                if (message.MessageType == MessageType.EVENT)
                                {
                                    await HandleEventMessage(message);
                                }
                            }
                        }
                        catch (Exception ex)
                        {

                        }
                    });

                }
            }
            catch (Exception ex)
            {

            }
        }

        private async Task HandleEventMessage(IDeviceMessage message)
        {
            var eventMessage = message as EventMessage;
            var byteBuffer = Unpooled.Buffer();
            if (eventMessage != null)
            {
                var outParams = new StringBuilder("");
                var outParamValue = new StringBuilder("");
                foreach (var item in eventMessage.Data)
                {
                    outParams.AppendFormat("{0}|", item.Key);
                    outParamValue.AppendFormat("{0}|", item.Value);
                }
                var model = new DeviceEventModel
                {
                    CreateDate = DateTimeOffset.Now,
                    DeviceId = eventMessage.DeviceId,
                    EventId = eventMessage.EventId,
                    EventOutParams = outParams.ToString(0, outParams.Length - 1),
                    EventOutParamValues = outParamValue.ToString(0, outParamValue.Length - 1),
                };
                await GetService<IDeviceEventService>().Add(model);
                var httpMessage = new HttpResultMessage<object>() { IsSucceed = true, StatusCode = (int)HttpStatus.Success };
                await Sender.SendAndFlushAsync(new TransportMessage(this.MessageId, httpMessage));
            }
            else
            {
                var httpMessage = new HttpResultMessage<object>() { IsSucceed = true, StatusCode = (int)HttpStatus.Success };
                await Sender.SendAndFlushAsync(new TransportMessage(this.MessageId, httpMessage));
            }
        }

        private async Task HandleReadPropertyMessage(IDeviceMessage message)
        {
            var readPropertyMessage = message as ReadPropertyMessage; 
            if (readPropertyMessage != null)
            {
                foreach (var item in readPropertyMessage.Properties)
                {
                    var model = new ReportPropertyModel
                    {
                        CreateDate = DateTimeOffset.Now,
                        DeviceId = readPropertyMessage.DeviceId,
                        PropertyId = item.Key,
                        PropertyValue = item.Value?.ToString(),

                    };
                    await GetService<IReportPropertyService>().Add(model);
                    await HandlePropertyLog(model);
                } 
            } 
        }

        private async Task ReplyMessage(DeviceMessageCodec messageCodec, IDeviceMessage message)
        {
            messageCodec.Encode(new MessageEncodeContext(message, async (deviceMessage) =>
            {
                var byteBuffer = Unpooled.Buffer();
                byteBuffer.WriteString(JsonSerializer.Serialize(deviceMessage, deviceMessage.GetType()), Encoding.UTF8);
                await Sender.SendAndFlushAsync(byteBuffer);
            })).Subscribe(async encodeMessage =>
            {
                var byteBuffer = Unpooled.Buffer();
                byteBuffer.WriteBytes(encodeMessage.GetBytes());
                await Sender.SendAndFlushAsync(byteBuffer);
            });
        }

        private async Task HandlePropertyLog(ReportPropertyModel model)
        {
            var propertyThresholdResult = await GetService<IPropertyThresholdService>().Get(new PropertyThresholdQuery
            {
                DeviceCode = _deviceId,
                ProductCode = _productCode,
                PropertyCode = model.PropertyId,

            });
            var propertyThresholds = propertyThresholdResult.Result;
            foreach (var threshold in propertyThresholds)
            {
                if (threshold.ThresholdLevel != "ignore")
                {
                    var reportPropertyLog = new ReportPropertyLogModel
                    {
                        DeviceCode = threshold.DeviceCode,
                        CreateDate = DateTimeOffset.Now,
                        Level = threshold.ThresholdLevel,
                        PropertyId = threshold.PropertyId,
                        PropertyValue = model.PropertyValue,
                        ThresholdValue = threshold.ThresholdValue,
                        ThresholdType = threshold.ThresholdType,
                        ProductCode = threshold.ProductCode,
                    };
                    await AddReportPropertyLog(reportPropertyLog);
                }
            }
        }

        private async Task<ApiResult<bool>> AddReportPropertyLog(ReportPropertyLogModel reportPropertyLog)
        {
            decimal.TryParse(reportPropertyLog.ThresholdValue, out decimal thresholdValue);
            ApiResult<bool> logResult = null;
            if (reportPropertyLog.ThresholdType == ">" && !(decimal.Parse(reportPropertyLog.PropertyValue) > thresholdValue))
            {
                logResult = await GetService<IReportPropertyLogService>().Add(reportPropertyLog);
            }
            if (reportPropertyLog.ThresholdType == "=" && !(decimal.Parse(reportPropertyLog.PropertyValue) == thresholdValue))
            {
                logResult = await GetService<IReportPropertyLogService>().Add(reportPropertyLog);
            }
            if (reportPropertyLog.ThresholdType == "<" && !(decimal.Parse(reportPropertyLog.PropertyValue) < thresholdValue))
            {
                logResult = await GetService<IReportPropertyLogService>().Add(reportPropertyLog);
            }
            if (reportPropertyLog.ThresholdType == "between")
            {
                var thresholdValues = reportPropertyLog.ThresholdValue.Split(",");
                if (thresholdValues.Length == 2 && !(decimal.Parse(reportPropertyLog.PropertyValue) > decimal.Parse(thresholdValues[0])
                && decimal.Parse(reportPropertyLog.PropertyValue) < decimal.Parse(thresholdValues[1])))
                {
                    logResult = await GetService<IReportPropertyLogService>().Add(reportPropertyLog);
                }
            }
            return logResult;
        }

        private async Task HandleOnlineMessage(IDeviceMessage message)
        {
            var token = message.Headers.GetValueOrDefault(DeviceOnlineMessage.LoginToken.Key);
            _deviceId = message.DeviceId;
            var deviceConfig = await GetService<IDeviceConfigService>().GetByDeviceCode(message.DeviceId);
            if (deviceConfig.Result != null)
            {
                _productCode = deviceConfig.Result.ProductCode;
                Sender.GetAndSet(_deviceProductKey, new Tuple<string, string>(_deviceId, _productCode));
                _registry.Register(new DeviceInfo
                {
                    Id = message.DeviceId,
                    ProductId = deviceConfig.Result.ProductCode,
                    Protocol = _deviceGatewayProperties.Protocol
                }).Subscribe(async deviceOperator =>
                {
                    var configMetadata = (DefaultConfigMetadata)await _protocolSupport.GetConfigMetadata(MessageTransport.Udp);
                    if (configMetadata != null)
                    {
                        var configValues = deviceConfig.Result.AuthConfig?.Split("|");
                        var configProperties = configMetadata.GetProperties();
                        for (var i = 0; i < configProperties.Count; i++)
                        {
                            var configValue = configValues != null && configValues.Length >= i ? configValues.GetValue(i) : default;
                            await deviceOperator.SetConfig(configProperties[i].Name, configValue);
                        }
                        deviceOperator.Authenticate(new DefaultAuthRequest(null,token?.ToString(), message.DeviceId,MessageTransport.Udp)).Subscribe(async authResult =>
                        {
                            if (authResult.Code == StatusCode.SUCCESS)
                            {
                                await _deviceSessionManager.AddorUpdate(message.DeviceId, p => new TcpDeviceSession(deviceOperator, Sender.GetClient(), MessageTransport.Udp));
                           
                            }
                            var byteBuffer = Unpooled.Buffer();
                            var options = new JsonSerializerOptions
                            {
                                Encoder = JavaScriptEncoder.Create(UnicodeRanges.All)
                            };
                            byteBuffer.WriteString(JsonSerializer.Serialize(authResult, options), Encoding.GetEncoding("gb2312"));
                            await Sender.SendAndFlushAsync(ByteBufferUtil.HexDump(byteBuffer));
                        });
                    }
                });
            }
        }
        public override void Load(UdpClient client, NetworkProperties tcpServerProperties)
        {
        }
    }
}

using Kayak.IModuleServices.DeviceReport;
using Surging.Core.DeviceGateway.Runtime.Core;
using Surging.Core.DeviceGateway.Runtime.Device.Implementation;
using Surging.Core.DeviceGateway.Runtime.Device;
using Surging.Core.DeviceGateway.Runtime.Session;
using Surging.Core.KestrelHttpServer.Runtime;
using Surging.Core.Protocol.Tcp.Runtime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Surging.Core.CPlatform.Protocol;
using System.Reactive.Linq;
using DotNetty.Buffers;
using Kayak.Core.Common.Response;
using Kayak.IModuleServices.DeviceManage.Models;
using Kayak.IModuleServices.DeviceManage.Queries;
using Kayak.IModuleServices.DeviceManage;
using Kayak.IModuleServices.System.Models;
using Kayak.IModuleServices.System;
using Surging.Core.DeviceGateway.Runtime.Core.Implementation;
using Surging.Core.DeviceGateway.Runtime.Core.Metadata;
using Surging.Core.DeviceGateway.Runtime.Device.Message.Property;
using Surging.Core.DeviceGateway.Runtime.Device.Message;
using Surging.Core.DeviceGateway.Runtime.device;
using Surging.Core.DeviceGateway.Runtime.Session.Implementation;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;
using Surging.Core.DeviceGateway.Runtime.Device.MessageCodec;
using Surging.Core.DeviceGateway.Runtime.Device.Implementation.Http;
using Microsoft.AspNetCore.Http;
using System.Net;
using HttpRequestMessage = Surging.Core.DeviceGateway.Runtime.Device.Implementation.Http.HttpRequestMessage;
using HttpMethod = Surging.Core.DeviceGateway.Runtime.Device.Implementation.Http.HttpMethod;
using Microsoft.AspNetCore.Http.Extensions;
using Surging.Core.CPlatform.Messages;
using HttpResponseMessage = Surging.Core.DeviceGateway.Runtime.Device.Implementation.Http.HttpResponseMessage;
using Google.Protobuf.WellKnownTypes;
using Surging.Core.DeviceGateway.Utilities;
using System.IO;
using Surging.Core.CPlatform;
using System.Collections.Concurrent;
using Kayak.Core.Http.Kestrel;
using Surging.Core.DeviceGateway.Runtime.Device.Message.Event;

namespace Kayak.Modules.DeviceReport.Domains
{
    public class HttpDeviceDataService : HttpBehavior, IHttpDeviceDataService
    {

        private readonly IDeviceProvider _deviceProvider;
        private IDeviceGateway _deviceGateway;
        private ProtocolSupport _protocolSupport;
        private IDeviceSessionManager _deviceSessionManager;
        private DeviceGatewayProperties _deviceGatewayProperties;
        private string _productCode;
        private readonly IDeviceRegistry _registry;
        private string _deviceId;

        public HttpDeviceDataService(IDeviceProvider deviceProvider, IDeviceRegistry registry, IDeviceGatewayManage deviceGatewayManage, IProtocolSupports protocolSupports, IDeviceSessionManager deviceSessionManager)
        {
            _deviceProvider = deviceProvider;

            _deviceSessionManager = deviceSessionManager;
            _registry = registry;
         
        }

        public override async Task<bool> Load(HttpServiceContext context)
        {
            var result = true;
            var deviceContext = context as HttpDeviceContext;
            if (deviceContext != null)
            {
                try
                {  
                    _deviceGatewayProperties=deviceContext.DeviceGatewayProperties;
                    _protocolSupport=deviceContext.ProtocolSupport;
                    _deviceGateway = deviceContext.DeviceGateway;
                    var complexProtocolSupport = _protocolSupport as ComplexProtocolSupport;
                    var deviceProduct = GetByPath(context.Path);
                    _productCode = deviceProduct?.Item1;
                    _deviceId = deviceProduct?.Item2;
                    var messageCodecSupport = await complexProtocolSupport?.GetMessageCodecSupport(_deviceGatewayProperties.Transport);
                    if (messageCodecSupport != null)
                    {
                        var httpRequest = await CovertHttpRequestMessage(context);
                        var deviceSession = _deviceId == null ? new HttpDeviceSession(Sender) : await _deviceSessionManager.GetSession(_deviceId);
                        if (deviceSession == null)
                        {
                            var deviceOnlineMsg = new DeviceOnlineMessage() { DeviceId = _deviceId };
                            deviceOnlineMsg.AddHeader("token", string.Join(";", httpRequest.GetHeader("Authorization").Value));
                            await HandleOnlineMessage(deviceOnlineMsg, context.Context.Connection);
                            deviceSession = await _deviceSessionManager.GetSession(_deviceId);
                        }
                        var httpExchange = new HttpExchangeMessage(httpRequest);
                        httpExchange.ResponseEvent += HttpExchange_ResponseEvent;
                        var message = messageCodecSupport.Decode(new MessageDecodeContext(_deviceId, httpExchange, deviceSession, _registry)).Subscribe(async message =>
                        {
                            try
                            {
                                if (message != null)
                                {
                                    if (message.MessageType == MessageType.ONLINE)
                                    {
                                        await HandleOnlineMessage(message, context.Context.Connection);
                                    }
                                    if (message.MessageType == MessageType.READ_PROPERTY)
                                    {
                                        await HandleReadPropertyMessage(message);
                                    }
                                    if (message.MessageType == MessageType.EVENT)
                                    {
                                        await HandleEventMessage(message, context.Path);
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
            return result;
        }

        private async Task HttpExchange_ResponseEvent(HttpResponseMessage message)
        {
           await Sender.SendAndFlushAsync(message.PayloadAsString(), message.Headers.ToDictionary(p => p.Name, p => string.Join(";", p.Value)));
        }

        private async Task<HttpRequestMessage> CovertHttpRequestMessage(HttpServiceContext context)
        {
            StreamReader streamReader = new StreamReader(context.Context.Request.Body);
            var data = await streamReader.ReadToEndAsync();
            return new HttpRequestMessage()
            {
                ContentType = context.Context.Request.ContentType ?? "",
                Method = new HttpMethod(context.Context.Request.Method),
                Path = context.Path,
                Url = context.Context.Request.GetEncodedUrl(),
                QueryParameters = context.Context.Request.Query.ToDictionary(p => p.Key, p => p.Value.ToString()),
                Headers = context.Context.Request.Headers.Select(p => new Header() { Name = p.Key, Value = p.Value.ToArray() }).ToList(),
                Payload = Unpooled.WrappedBuffer(Encoding.UTF8.GetBytes( data))
            };
        }

        private async Task HandleReadPropertyMessage(IDeviceMessage message)
        {
            var readPropertyMessage = message as ReadPropertyMessage;
            var byteBuffer = Unpooled.Buffer();
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
                byteBuffer.WriteByte(1);
                var httpMessage=new HttpResultMessage<object>() { IsSucceed = true , StatusCode=(int)HttpStatus.Success};
              await Sender.SendAndFlushAsync(new TransportMessage(this.MessageId, httpMessage));
            }
            else
            {
                byteBuffer.WriteByte(0);

                var httpMessage = new HttpResultMessage<object>() { IsSucceed = true, StatusCode = (int)HttpStatus.Success };
                await Sender.SendAndFlushAsync(new TransportMessage(this.MessageId, httpMessage));
            }
        }

        private async Task HandleEventMessage(IDeviceMessage message,string path)
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
                    outParamValue.AppendFormat("{0}|",item.Value);
                }
                var model = new DeviceEventModel
                {
                    CreateDate = DateTimeOffset.Now,
                    DeviceId = eventMessage.DeviceId,
                    EventId = string.IsNullOrEmpty(eventMessage.EventId) ? GetEventByPath(path) : eventMessage.EventId,
                    EventOutParams = outParams.ToString(0,outParams.Length-1),
                    EventOutParamValues = outParamValue.ToString(0, outParamValue.Length-1),
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

        private Tuple<string,string> GetByPath(string path)
        {
           var paths= path.Split("/");
            if(paths.Length >2)
            return new (paths[0], paths[1]);
            return null;
        }

        private string GetEventByPath(string path)
        {
            var paths = path.Split("/");
            if (paths.Length > 3)
                return new(paths[3]);
            return null;
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

        private async Task HandleOnlineMessage(IDeviceMessage message, ConnectionInfo connectionInfo)
        {
            var remoteEndPoint = connectionInfo.RemoteIpAddress == null ? null : new IPEndPoint(connectionInfo.RemoteIpAddress, connectionInfo.RemotePort);
            var token = message.Headers.GetValueOrDefault(DeviceOnlineMessage.LoginToken.Key);
            _deviceId = message.DeviceId;
            var deviceConfig = await GetService<IDeviceConfigService>().GetByDeviceCode(message.DeviceId);
            if (deviceConfig.Result != null)
            {
                _productCode = deviceConfig.Result.ProductCode;
                _registry.Register(new DeviceInfo
                {
                    Id = message.DeviceId,
                    ProductId = deviceConfig.Result.ProductCode,
                    Protocol = _deviceGatewayProperties.Protocol
                }).Subscribe(async deviceOperator =>
                {
                    var configMetadata = (DefaultConfigMetadata)await _protocolSupport.GetConfigMetadata(MessageTransport.Http);
                    if (configMetadata != null)
                    {
                        var configValues = deviceConfig.Result.AuthConfig?.Split("|");
                        var configProperties = configMetadata.GetProperties();
                        for (var i = 0; i < configProperties.Count; i++)
                        {
                            var configValue = configValues != null && configValues.Length >= i ? configValues.GetValue(i) : default;
                            await deviceOperator.SetConfig(configProperties[i].Name, configValue);
                        }
                        deviceOperator.Authenticate(new DefaultAuthRequest(null, token?.ToString(), message.DeviceId, MessageTransport.Http)).Subscribe(async authResult =>
                        {
                            if (authResult.Code == StatusCode.SUCCESS)
                            {
                                await _deviceSessionManager.AddorUpdate(message.DeviceId, p => new HttpDeviceSession(deviceOperator, remoteEndPoint,Sender));
                            }
                        });
                    }
                });
            }
        }
    }
}

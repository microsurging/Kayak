using Surging.Core.CPlatform.Protocol;
using Surging.Core.DeviceGateway.Runtime.Core.Metadata.Type;
using Surging.Core.DeviceGateway.Runtime.Core.Metadata;
using Surging.Core.DeviceGateway.Runtime.Core;
using Surging.Core.DeviceGateway.Runtime.Device.Implementation;
using Surging.Core.DeviceGateway.Runtime.Device.Message;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using Surging.Core.DeviceGateway.Runtime.Core.Implementation;
using Surging.Core.DeviceGateway.Runtime.Device;
using Surging.Core.DeviceGateway.Runtime.Device.MessageCodec;
using Surging.Core.CPlatform;

namespace Kayak.Core.Codecs
{
    public class Demo5ProtocolSupportProvider : ProtocolSupportProvider
    {
        private readonly DefaultConfigMetadata _tcpConfig = new DefaultConfigMetadata(
        "TCP认证配置"
        , "key为tcp认证密钥")
        .Add("tcp_auth_key", "key", "TCP认证KEY", StringType.Instance);


        private readonly DefaultConfigMetadata _httpConfig = new DefaultConfigMetadata(
        "Http认证配置"
        , "token为http认证令牌")
        .Add("token", "token", "http令牌", StringType.Instance);

        private readonly DefaultConfigMetadata _mqttConfig = new DefaultConfigMetadata(
        "Mqtt认证配置"
        , "secureId以及secureKey在创建设备产品或设备实例时进行配置.\r\n    timestamp为当前时间戳(毫秒), 与服务器时间不能相差5分钟.\r\n        md5为32位, 不区分大小写")
        .Add("secureId", "secureId", "用户唯一标识编号", StringType.Instance)
        .Add("secureKey", "secureKey", "密钥", StringType.Instance);
        public override IObservable<ProtocolSupport> Create(ProtocolContext context)
        {
            var support = new ComplexProtocolSupport();
            support.Id = "demo5";
            support.Name = "演示协议5";
            support.Description = "演示协议5";
            support.AddAuthenticator(MessageTransport.Tcp, new Demo5Authenticator());
            support.AddDocument(MessageTransport.Tcp, "Document/document-tcp.md");
            support.Script = "\r\nvar decode=function(buffer)\r\n{\r\n  parser.Fixed(5).Handler(\r\n function(buffer){ \r\n      var bytes = BytesUtils.GetBytes(buffer,1,4);\r\n      var len = BytesUtils.LeStrToInt(bytes,1,4);//2. 获取消息长度.\r\n       var buf = BytesUtils.Slice(buffer,0,5); \r\n parser.Fixed(len).Result(buf); \r\n        }).Handler(function(buffer){ parser.Result(buffer).Complete();  \r\n        }\r\n )\r\n}\r\nvar encode=function(buffer)\r\n{\r\n}";
            support.AddMessageCodecSupport(MessageTransport.Tcp, () => Observable.Return(new ScriptDeviceMessageCodec(support.Script)));
            support.AddConfigMetadata(MessageTransport.Tcp, _tcpConfig);

            support.AddDocument(MessageTransport.Mqtt, "Document/document-mqtt.md");
            support.AddAuthenticator(MessageTransport.Mqtt, new DefaultAuthenticator());
            support.Script = "\r\nvar decode=function(buffer)\r\n{\r\n  parser.Fixed(5).Handler(\r\n function(buffer){ \r\n      var bytes = BytesUtils.GetBytes(buffer,1,4);\r\n      var len = BytesUtils.LeStrToInt(bytes,1,4);//2. 获取消息长度.\r\n       var buf = BytesUtils.Slice(buffer,0,5); \r\n parser.Fixed(len).Result(buf); \r\n        }).Handler(function(buffer){ parser.Result(buffer).Complete();  \r\n        }\r\n )\r\n}\r\nvar encode=function(buffer)\r\n{\r\n}";
            support.AddMessageCodecSupport(MessageTransport.Mqtt, () => Observable.Return(new ScriptDeviceMessageCodec(support.Script)));
            support.AddConfigMetadata(MessageTransport.Mqtt, _mqttConfig);

            support.AddDocument(MessageTransport.Http, "Document/document-http.md");
            support.AddAuthenticator(MessageTransport.Http, new Demo5Authenticator()); 
            support.AddRoutes(MessageTransport.Http, new List<TopicMessageCodec>() {
               TopicMessageCodec.DeviceOnline,
                TopicMessageCodec.ReportProperty,
                TopicMessageCodec.WriteProperty,
                 TopicMessageCodec.ReadProperty,
                   TopicMessageCodec.Event
            }.Select(p => HttpDescriptor.Instance(p.Pattern)
                .GroupName(p.Route.GroupName())
                .HttpMethod(p.Route.HttpMethod())
                .Path(p.Pattern)
                .ContentType(MediaType.ToString(MediaType.ApplicationJson))
                .Description(p.Route.Description())
                .Example(p.Route.Example())
                ).ToList());
            support.AddMessageCodecSupport(MessageTransport.Http, () => Observable.Return(new HttpDeviceMessageCodec()));
            support.AddConfigMetadata(MessageTransport.Http, _httpConfig);
            return Observable.Return(support);
        }

        public class Demo5Authenticator : IAuthenticator
        {
            public IObservable<AuthenticationResult> Authenticate(IAuthenticationRequest request, IDeviceOperator deviceOperator)
            {
                var result = Observable.Return<AuthenticationResult>(default);
                if (request is DefaultAuthRequest)
                {
                    var authRequest = request as DefaultAuthRequest;
                    deviceOperator.GetConfig(authRequest.GetTransport()==MessageTransport.Http?"token": "key").Subscribe(  config =>
                    {
                        var password = config.Convert<string>();
                        if (authRequest.Password.Equals(password))
                        {
                            result= result.Publish(AuthenticationResult.Success(authRequest.DeviceId));
                        }
                        else
                        {
                            result= result.Publish(AuthenticationResult.Failure(StatusCode.CUSTOM_ERROR, "验证失败,密码错误"));
                        }
                    });
                }
                else
                result = Observable.Return<AuthenticationResult>(AuthenticationResult.Failure(StatusCode.CUSTOM_ERROR, "不支持请求参数类型"));
                return result;
            }

            public IObservable<AuthenticationResult> Authenticate(IAuthenticationRequest request, IDeviceRegistry registry)
            {
                var result = Observable.Return<AuthenticationResult>(default);
                var authRequest = request as DefaultAuthRequest;
                registry
                  .GetDevice(authRequest.DeviceId)
                  .Subscribe(async p => {

                     var config=  await p.GetConfig(authRequest.GetTransport() == MessageTransport.Http ? "token" : "key");
                      var password= config.Convert<string>();
                     if(authRequest.Password.Equals(password))
                      {
                          result= result.Publish(AuthenticationResult.Success(authRequest.DeviceId));
                      }
                      else
                      {
                          result= result.Publish(AuthenticationResult.Failure(StatusCode.CUSTOM_ERROR, "验证失败,密码错误"));
                      }
                  });
                return result;
            }
        }
    }
}
using DotNetty.Common.Utilities;
using Surging.Core.CPlatform.Protocol;
using Surging.Core.DeviceGateway.Runtime.Core;
using Surging.Core.DeviceGateway.Runtime.Core.Implementation;
using Surging.Core.DeviceGateway.Runtime.Core.Metadata;
using Surging.Core.DeviceGateway.Runtime.Core.Metadata.Type;
using Surging.Core.DeviceGateway.Runtime.Device;
using Surging.Core.DeviceGateway.Runtime.Device.Implementation;
using Surging.Core.DeviceGateway.Runtime.Device.Message.Property;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using static Kayak.Core.Codecs.Demo5ProtocolSupportProvider;

namespace Kayak.Core.Codecs
{
    public class Demo3ProtocolSupportProvider : ProtocolSupportProvider
    {
        private readonly DefaultConfigMetadata _tcpConfig = new DefaultConfigMetadata(
        "TCP认证配置"
        , "key为tcp认证密钥")
        .Add("tcp_auth_key", "key", "TCP认证KEY", StringType.Instance);
        public override IObservable<ProtocolSupport> Create(ProtocolContext context)
        {

            var support = new ComplexProtocolSupport();
            support.Id = "demo_3";
            support.Name = "演示协议3";
            support.Description = "演示协议3";
            support.AddAuthenticator(MessageTransport.Tcp, new Demo5Authenticator());
            support.AddDocument(MessageTransport.Tcp, "Document/document-tcp.md");
            support.Script = "\r\nvar decode=function(buffer)\r\n{\r\n  parser.Fixed(5).Handler(\r\n function(buffer){ \r\n      var bytes = BytesUtils.GetBytes(buffer,1,4);\r\n      var len = BytesUtils.LeStrToInt(bytes,1,4);//2. 获取消息长度.\r\n       var buf = BytesUtils.Slice(buffer,0,5); \r\n parser.Fixed(len).Result(buf); \r\n        }).Handler(function(buffer){ parser.Result(buffer).Complete();  \r\n        }\r\n )\r\n}\r\nvar encode=function(buffer)\r\n{\r\n}";       
            support.AddMessageCodecSupport(MessageTransport.Tcp, () => Observable.Return(new ScriptDeviceMessageCodec(support.Script)));
            support.AddConfigMetadata(MessageTransport.Tcp, _tcpConfig);
            ReadPropertyMessage deviceOnlineMessage= new ReadPropertyMessage();
            deviceOnlineMessage.DeviceId = "scro-34";
            deviceOnlineMessage.AddProperties("temp", "38.24");
           var v=JsonSerializer.Serialize( deviceOnlineMessage);
            return Observable.Return(support);
        }

        public class Demo5Authenticator : IAuthenticator
        {
            public IObservable<AuthenticationResult> Authenticate(IAuthenticationRequest request, IDeviceOperator deviceOperator)
            {
                var result = Observable.Return<AuthenticationResult>(AuthenticationResult.Failure(StatusCode.CUSTOM_ERROR, "不支持请求参数类型")); 
                if (request is DefaultAuthRequest)
                {
                    var authRequest = request as DefaultAuthRequest;
                    deviceOperator.GetConfig("key").Subscribe(config =>
                    {
                        var password = config.Convert<string>();
                        if (authRequest.Password.Equals(password))
                        {
                            result = result.Publish(AuthenticationResult.Success(authRequest.DeviceId));
                        }
                        else
                        {
                            result = result.Publish(AuthenticationResult.Failure(StatusCode.CUSTOM_ERROR, "验证失败,密码错误"));
                        }
                    });
                }
                return result;
            }

            public IObservable<AuthenticationResult> Authenticate(IAuthenticationRequest request, IDeviceRegistry registry)
            {
                var result = Observable.Return<AuthenticationResult>(default);
                var authRequest = request as DefaultAuthRequest;
                registry
                  .GetDevice(authRequest.DeviceId)
                  .Subscribe( async p => {

                      var config = await p.GetConfig("key");
                      var password = config.Convert<string>();
                      if (authRequest.Password.Equals(password))
                      {
                          result = result.Publish(AuthenticationResult.Success(authRequest.DeviceId));
                      }
                      else
                      {
                          result = result.Publish(AuthenticationResult.Failure(StatusCode.CUSTOM_ERROR, "验证失败,密码错误"));
                      }
                  });
                return result;
            }
        }
    }
}
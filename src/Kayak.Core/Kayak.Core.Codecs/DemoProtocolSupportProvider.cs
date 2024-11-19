using Surging.Core.CPlatform.Protocol;
using Surging.Core.DeviceGateway.Runtime.Core;
using Surging.Core.DeviceGateway.Runtime.Device.Implementation;
using System;
using System.Reactive.Linq;

namespace Kayak.Core.Codecs
{
    public class DemoProtocolSupportProvider : ProtocolSupportProvider
    {
        public override IObservable<ProtocolSupport> Create(ProtocolContext context)
        {

            var support = new ComplexProtocolSupport();
            support.Id = "demo_v2";
            support.Name = "演示协议2";
            support.Description = "演示协议2";
            support.AddMessageCodecSupport(MessageTransport.Tcp, () => Observable.Return(new DemoTcpMessageCodec()));
          //  var n = new ScriptDeviceMessageCodec("var messagetype=1;\r\nvar decode=function(buffer)\r\n{\r\n  parser.Fixed(5).Handler(\r\n function(buffer){ \r\n      var bytes = BytesUtils.GetBytes(buffer,1,4);\r\n      var len = BytesUtils.LeStrToInt(bytes,1,4);//2. 获取消息长度.\r\n   parser.Fixed(len).Result(buffer); \r\n        }).Handler(function(buffer){ parser.Result(buffer).Complete();  \r\n        }\r\n )\r\n}\r\nvar encode=function(buffer)\r\n{\r\n}");
            return Observable.Return(support);
        }
    }
}

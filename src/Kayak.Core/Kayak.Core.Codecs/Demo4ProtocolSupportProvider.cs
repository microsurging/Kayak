using Surging.Core.CPlatform.Protocol;
using Surging.Core.DeviceGateway.Runtime.Core;
using Surging.Core.DeviceGateway.Runtime.Device.Implementation;
using Surging.Core.DeviceGateway.Runtime.Device.Message;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kayak.Core.Codecs
{
    public class Demo4ProtocolSupportProvider : ProtocolSupportProvider
    {
        public override IObservable<ProtocolSupport> Create(ProtocolContext context)
        {
            var support = new ComplexProtocolSupport();
            support.Id = "demo_4";
            support.Name = "演示协议4";
            support.Description = "演示协议4";

            support.Script = "\r\nvar decode=function(buffer)\r\n{\r\n  parser.Fixed(5).Handler(\r\n function(buffer){ \r\n      var bytes = BytesUtils.GetBytes(buffer,1,4);\r\n      var len = BytesUtils.LeStrToInt(bytes,1,4);//2. 获取消息长度.\r\n       var buf = BytesUtils.Slice(buffer,0,5); \r\n parser.Fixed(len).Result(buf); \r\n        }).Handler(function(buffer){ parser.Result(buffer).Complete();  \r\n        }\r\n )\r\n}\r\nvar encode=function(buffer)\r\n{\r\n}";
            support.AddMessageCodecSupport(MessageTransport.Udp, () => Observable.Return(new ScriptDeviceMessageCodec(support.Script)));
           
            return Observable.Return(support);
        }
    }
}
using Microsoft.AspNetCore.Http;
using Surging.Core.CPlatform.Messages;
using Surging.Core.CPlatform.Transport;
using Surging.Core.DeviceGateway.Runtime.Core;
using Surging.Core.DeviceGateway.Runtime.Device.Implementation;
using Surging.Core.KestrelHttpServer.Interceptors;
using Surging.Core.KestrelHttpServer.Interceptors.Implementation;
using Surging.Core.KestrelHttpServer.Runtime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kayak.Core.Http.Kestrel
{
    internal class HttpDeviceInvocation: HttpInvocation
    {

        public IHttpMessageSender Sender { get; internal set; }
        public HttpServiceEntry Entry { get; internal set; }
        public string Path { get; internal set; }

        public HttpResultMessage<object> Result { get; set; }

        public HttpContext Context { get; internal set; }

        public string NetworkId { get; internal set; }
        public DeviceGatewayProperties DeviceGatewayProperties { get; internal set; }

        public IDeviceGateway DeviceGateway { get; internal set; }

        public ProtocolSupport ProtocolSupport { get; internal set; }

        public override async Task<bool> Proceed()
        { 
            var httpBehavior = Entry.Behavior();
            httpBehavior.Sender = Sender;
            httpBehavior.NetworkId.OnNext(NetworkId);
            return await httpBehavior.Load(new HttpDeviceContext(Path, Context)
            {
                  DeviceGateway=this.DeviceGateway,
                  ProtocolSupport=ProtocolSupport,
                   DeviceGatewayProperties=DeviceGatewayProperties
            });
        }
    }
}

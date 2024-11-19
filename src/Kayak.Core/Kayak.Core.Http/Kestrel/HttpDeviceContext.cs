using Microsoft.AspNetCore.Http;
using Surging.Core.DeviceGateway.Runtime.Core;
using Surging.Core.DeviceGateway.Runtime.Device.Implementation;
using Surging.Core.KestrelHttpServer.Runtime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kayak.Core.Http.Kestrel
{
    public class HttpDeviceContext: HttpServiceContext
    {

        public HttpDeviceContext(string path, HttpContext context):base(path, context) { }
        public DeviceGatewayProperties DeviceGatewayProperties { get; internal set; }

        public IDeviceGateway DeviceGateway { get; internal set; }

        public ProtocolSupport ProtocolSupport { get; internal set; }

    }
}

using Greet;
using Grpc.Core;
using Surging.Core.CPlatform.Ioc;
using Surging.Core.CPlatform.Runtime.Server.Implementation.ServiceDiscovery.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kayak.IModuleServices.DeviceReport
{
    [ServiceBundle("api/{Service}/{Method}")]
    public interface IGrpcDeviceDataService : IServiceKey
    {
        Task<DeviceReply> ChangeDeviceStage(DeviceRequest request, ServerCallContext context=null);
    }
}

using Greet;
using Grpc.Core;
using Kayak.IModuleServices.DeviceReport;
using Kayak.Modules.DeviceReport.Behaviors;

namespace Kayak.Modules.DeviceReport.Domains
{
    public class GrpcDeviceDataService : GreeterBehavior, IGrpcDeviceDataService
    {
        public override Task<DeviceReply> ChangeDeviceStage(DeviceRequest request, ServerCallContext context)
        {
            return Task.FromResult(new DeviceReply
            {
                Message = true
            }) ;
        }
    }
}

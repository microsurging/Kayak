using Surging.Core.CPlatform.Ioc;
using Surging.Core.CPlatform.Runtime.Server.Implementation.ServiceDiscovery.Attributes;
using System.ServiceModel;

namespace Kayak.IModuleServices.DeviceReport
{
    [ServiceBundle("DeviceData/{Service}")]
    [ServiceContract]
    public interface IDeviceDataService : IServiceKey
    {
        [OperationContract]
        Task<bool> ChangeDeviceStage(string deviceId);

        [OperationContract]
        Task<bool> ChangeDeviceStage1(string deviceId);
    }
}

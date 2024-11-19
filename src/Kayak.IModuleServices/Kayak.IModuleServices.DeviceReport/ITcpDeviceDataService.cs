using Surging.Core.CPlatform.Ioc;
using Surging.Core.CPlatform.Runtime.Server.Implementation.ServiceDiscovery.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Kayak.IModuleServices.DeviceReport
{
    [ServiceBundle("DeviceData/{Service}")]
    public interface ITcpDeviceDataService : IServiceKey
    {
        [OperationContract]
        Task<bool> ChangeDeviceStage(string deviceId);
    }
} 

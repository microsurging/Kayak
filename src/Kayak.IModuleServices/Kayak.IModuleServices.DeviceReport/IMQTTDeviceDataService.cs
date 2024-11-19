using Kayak.IModuleServices.DeviceReport.Models;
using Surging.Core.CPlatform.Ioc;
using Surging.Core.CPlatform.Runtime.Client.Address.Resolvers.Implementation.Selectors.Implementation;
using Surging.Core.CPlatform.Runtime.Server.Implementation.ServiceDiscovery.Attributes;
using Surging.Core.CPlatform.Support.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kayak.IModuleServices.DeviceReport
{
    [ServiceBundle("Device/{Service}")]
    public interface IMQTTDeviceDataService : IServiceKey
    {
        [Command(ShuntStrategy = AddressSelectorMode.HashAlgorithm)]
        Task Publish(string deviceId, WillMessage message);

        [Command(ShuntStrategy = AddressSelectorMode.HashAlgorithm)]
        Task<bool> IsOnline(string deviceId);
    }
}
 

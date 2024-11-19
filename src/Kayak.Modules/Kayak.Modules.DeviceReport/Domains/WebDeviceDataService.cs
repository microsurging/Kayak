using Kayak.IModuleServices.DeviceReport;
using Surging.Core.CPlatform.Ioc;
using Surging.Core.Protocol.WebService.Runtime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kayak.Modules.DeviceReport.Domains
{
    [ModuleName("WebService")]
    internal class IWebDeviceDataService : WebServiceBehavior, IDeviceDataService
    {
        public Task<bool> ChangeDeviceStage(string deviceId)
        {
            return Task.FromResult(true);
        }

        public Task<bool> ChangeDeviceStage1(string deviceId)
        {
            return Task.FromResult(true);
        }
    }
}

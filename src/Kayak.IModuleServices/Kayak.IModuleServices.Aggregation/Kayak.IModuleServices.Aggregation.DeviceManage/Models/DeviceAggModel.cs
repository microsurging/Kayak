using Kayak.IModuleServices.DeviceManage.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kayak.IModuleServices.Aggregation.DeviceManage.Models
{
    public class DeviceAggModel: DeviceModel
    {
        public string ProductName { get; set; }

        public string GatewayName { get; set; }

        public string ConnProtocol { get; set; }

        public string ProtocolName { get; set; }

        public string IpAddress {  get; set; }
    }
}

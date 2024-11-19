using Kayak.IModuleServices.DeviceAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kayak.IModuleServices.Aggregation.DeviceAccess.Models
{
    public class DeviceGatewayAggModel: DeviceGatewayModel
    {
        public GatewayTypeDictionary GatewayType { get; set; }

        public NetworkPartModel NetworkPart { get; set; }

        public string IpAddress
        {
            get
            {
                if (NetworkPart != null && NetworkPart != null)
                {
                    return $"{NetworkPart.ComponentTypeCode.ToLower()}://{NetworkPart.Host}:{NetworkPart.Port}";
                }
                return "";
            }
        }

        public ProtocolModel Protocol { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kayak.IModuleServices.DeviceManage.Models
{
    public class DeviceAccessModel
    {
        public int Id { get; set; } 
        public int GatewayId { get; set; }

        public string ProductCode { get; set; }

        public string GatewayName { get; set; }
        public string Remark { get; set; }
        public string ConnProtocol { get; set; }

        public string? AuthConfig { get; set; }

        public DateTimeOffset? CreateDate { get; set; } = DateTimeOffset.Now;

        public DateTimeOffset? UpdateDate { get; set; } = DateTimeOffset.Now;
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kayak.DataAccess.DeviceData.Entities
{
    [Table("Component_DeviceAccess")]
    public class DeviceAccess : EntityFull
    {
        public int GatewayId {  get; set; }

        public string ProductCode { get; set; }

        public string GatewayName { get; set;}

        public string ConnProtocol { get; set; }

        public string? AuthConfig{ get; set; }
    }
}

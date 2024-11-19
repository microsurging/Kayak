using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kayak.DataAccess.DeviceData.Entities
{
    [Table("Component_DeviceGateway")]
    public class DeviceGateway : EntityFull
    {
        public string Name { get; set; }

        public string GatewayTypeValue { get; set; }

        public int Status { get; set; }

        public int NetWorkId { get; set; }

        public string ProtocolCode { get; set; }
    }
}

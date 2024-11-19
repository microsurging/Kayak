using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kayak.IModuleServices.DeviceAccess.Models
{
    public class DeviceGatewayModel
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public string GatewayTypeValue { get; set; }

        public int NetWorkId { get; set; }

        public int Status { get; set; }

        public string ProtocolCode { get; set; }

        public string Remark { get; set; }

        public DateTimeOffset? CreateDate { get; set; }= DateTimeOffset.Now;
        public DateTimeOffset? UpdateDate { get; set; } = DateTimeOffset.Now;
    }
}

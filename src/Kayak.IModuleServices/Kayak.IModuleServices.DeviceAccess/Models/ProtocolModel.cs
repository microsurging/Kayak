using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kayak.IModuleServices.DeviceAccess.Models
{
    public class ProtocolModel
    {
        public int Id { get; set; }
        public string ProtocolCode { get; set; }

        public string ProtocolName { get; set; }

        public string ProtocolType { get; set; }

        public string ClassName { get; set; }

        public string? Script { get; set; }

        public string ConnProtocol { get; set; }

        public int  Status { get; set; }

        public string Remark { get;set; }

        public string FileAddress { get; set; }

        public DateTimeOffset? CreateDate { get; set; }= DateTimeOffset.Now;

        public DateTimeOffset? UpdateDate { get; set; } = DateTimeOffset.Now;
    }
}

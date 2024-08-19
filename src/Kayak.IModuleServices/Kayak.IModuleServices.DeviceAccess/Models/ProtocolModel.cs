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

        public string ComponentType { get; set; }

        public string ClassName { get; set; }

        public int  Status { get; set; }

        public string Remark { get;set; }

        public string FileAddress { get; set; }

        public DateTime? CreateDate { get; set; }=DateTime.Now;

        public DateTime? UpdateDate { get; set; } = DateTime.Now;
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kayak.DataAccess.DeviceData.Entities
{
    [Table("Sys_NetworkLog")]
    public class NetworkLog : EntityFull
    {
        public string NetworkId { get; set; }

        public int logLevel { get; set; }

        public int NetworkType { get; set; }

        public string? EventName { get; set; }

        public string Content { get; set; }
         
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kayak.DataAccess.DeviceData.Entities
{
    [Table("Sys_OperateLog")]
    public class OperateLog : EntityFull
    {
        public string ServiceId { get; set; }

        public string RoutePath { get; set; }

        public string Arguments { get; set; }

        public string ReturnType { get; set; }

        public string? Payload { get; set; }

        public long? RunTime { get; set; }

        public string ReturnValue { get; set; }
    }
}

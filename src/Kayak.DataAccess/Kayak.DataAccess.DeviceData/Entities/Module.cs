using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kayak.DataAccess.DeviceData.Entities
{
    [Table("Component_Module")]
    public class Module : EntityFull
    {
        public string ModuleCode { get; set; }

        public string ModuleName { get; set; }

        public string ModuleType { get; set; }

        public string ModuleMode { get; set; }

        public string? FileAddress { get; set; }
    }
}

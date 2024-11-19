using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kayak.DataAccess.DeviceData.Entities
{
    [Table("Sys_Unit")]
    public class SysUnit : EntityFull
    {
        [Description("名称")]
        [Column("Name", TypeName = "varchar(50)")]
        public string Name { get; set; }


        [Description("数值")]
        [Column("Value")]
        public int? Value { get; set; }
    }
}

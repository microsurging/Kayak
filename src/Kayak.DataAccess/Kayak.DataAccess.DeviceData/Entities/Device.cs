using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kayak.DataAccess.DeviceData.Entities
{
    [Table("Component_Device")]
    public class Device : EntityFull
    {
        public string Code { get; set; }

        public string Name { get; set; }

        public string ProductCode{ get;set; }


        [Description("状态")]
        [Column("State")]
        public int Status { get; set; } = 1;
    }
}

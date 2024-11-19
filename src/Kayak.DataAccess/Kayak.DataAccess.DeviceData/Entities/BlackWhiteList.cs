using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kayak.DataAccess.DeviceData.Entities
{
    [Table("Component_BlackWhiteList")]
    public class BlackWhiteList : EntityFull
    {
        public string RoutePathPattern { get; set; }

        public string? BlackList { get; set; }

        public string? WhiteList { get; set; }

        public int Status { get; set; }
    }
}

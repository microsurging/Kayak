using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kayak.DataAccess.DeviceData.Entities
{
    [Table("Physical_EventParameter")]
    public class EventParameter : EntityFull
    {
        public int EventId { get; set; }

        public string EventCode { get; set; }

        public string? DeviceCode { get; set; }

        public string ProductCode { get; set; }

        public string Code { get; set; }

        public string Name { get; set; }

        public string DataTypeValue { get; set; }

    }
}

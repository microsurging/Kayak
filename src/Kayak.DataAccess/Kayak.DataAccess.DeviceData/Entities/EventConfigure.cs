using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kayak.DataAccess.DeviceData.Entities
{
    [Table("Physical_EventConfigure")]
    public  class EventConfigure : EntityFull
    {
        public string EventId { get; set; }

        public string EventName { get; set; }

        public string DataTypeValue { get; set; }

        public string? Eventlevel { get; set; }

        public string CorrelationId { get; set; }

        public string CorrelationFrom { get; set; }

        public string? Expands { get; set; }
    }
}

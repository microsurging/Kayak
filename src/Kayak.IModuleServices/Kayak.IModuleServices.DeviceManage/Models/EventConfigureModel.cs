using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kayak.IModuleServices.DeviceManage.Models
{
    public class EventConfigureModel
    {
        public int Id {  get; set; }
        public string EventId { get; set; }

        public string EventName { get; set; }

        public string? Eventlevel { get; set; }

        public string DataTypeValue { get; set; }

        public string CorrelationId { get; set; }

        public string CorrelationFrom { get; set; }

        public string? Expands { get; set; }

        public string Remark { get; set; }

        public DateTimeOffset? CreateDate { get; set; } = DateTimeOffset.Now;

        public DateTimeOffset? UpdateDate { get; set; } = DateTimeOffset.Now;
    }
}

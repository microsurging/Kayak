using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kayak.IModuleServices.DeviceManage.Models
{
    public class EventParameterModel
    {
        public int? Id { get; set; }
        public int EventId { get; set; }

        public string EventCode { get; set; }

        public string? DeviceCode { get; set; }

        public string ProductCode { get; set; }

        public string Code { get; set; }

        public string Name { get; set; }

        public string DataTypeValue { get; set; }

        public DateTimeOffset? CreateDate { get; set; } = DateTimeOffset.Now;
        public DateTimeOffset? UpdateDate { get; set; } = DateTimeOffset.Now;
    }
}

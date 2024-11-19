using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kayak.IModuleServices.Aggregation.DeviceManage.Models
{
    public class ReportPropertyAggModel
    {
        public int Id {  get; set; }
        public string DeviceId { get; set; }

        public string PropertyId { get; set; }

        public string? UnitValue { get; set; }

        public string PropertyName {  get; set; }

        public string? PropertyValue { get; set; }

        public DateTimeOffset? CreateDate { get; set; } = DateTimeOffset.Now;
    }
}

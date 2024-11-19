using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kayak.IModuleServices.System.Models
{
    public class ReportPropertyLogModel
    {
        public int Id { get; set; }
        public int PropertyId { get; set; }

        public string DeviceCode { get; set; }

        public string PropertyValue { get; set; }

        public string ThresholdValue { get; set; }

        public string Level { get; set; }

        public string? Content { get; set; }

        public string ProductCode { get; set; }
        public string ThresholdType { get; set; }
        public string ThresholdCondition { get; set; }

        public int Status { get; set; }

        public DateTimeOffset? CreateDate { get; set; } = DateTimeOffset.Now;

        public DateTimeOffset? UpdateDate { get; set; } = DateTimeOffset.Now;
    }
}

using Kayak.IModuleServices.DeviceManage.Models;
using Surging.Core.System.Intercept;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kayak.IModuleServices.DeviceManage.Models
{
    public class PropertyThresholdModel
    {
        public int? Id { get; set; }
        public int PropertyId { get; set; }

        public string PropertyCode { get; set; }
        public string DeviceCode { get; set; }

        public string ProductCode { get; set; }
        public string ThresholdValue { get; set; }

        public string ThresholdType { get; set; }

        public string ThresholdLevel { get; set; }

        public DateTimeOffset? CreateDate { get; set; } = DateTimeOffset.Now;
        public DateTimeOffset? UpdateDate { get; set; } = DateTimeOffset.Now;
    }
}

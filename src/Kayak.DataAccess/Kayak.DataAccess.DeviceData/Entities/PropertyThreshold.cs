using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kayak.DataAccess.DeviceData.Entities
{
    [Table("Physical_PropertyThreshold")]
    public class PropertyThreshold : EntityFull
    {
        public int PropertyId { get; set; }

        public string PropertyCode { get; set; }

        public string? DeviceCode { get; set; }

        public string ProductCode { get; set; }


        public string ThresholdValue { get; set; }

        public string ThresholdType { get; set; }

        public string ThresholdLevel { get; set; }
    }
}

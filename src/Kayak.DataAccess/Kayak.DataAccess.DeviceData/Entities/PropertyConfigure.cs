using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kayak.DataAccess.DeviceData.Entities
{
    [Table("Physical_PropertyConfigure")]
    public class PropertyConfigure : EntityFull
    {
        public string PropertyId { get; set; }

        public string PropertyName { get; set; }

        public string? DataTypeValue { get; set; }

        public int Precision { get; set; }
        public string SourceValue { get; set; }
        public string? DefaultValue { get; set; }

        public int? MaxLength { get; set; }

        public string ReadWrite { get; set; }

        public string? ValueRange { get; set; }

        public float? Step { get; set; }

        public string? UnitValue { get; set; }


        public string CorrelationId { get; set; }

        public string CorrelationFrom { get; set; }
    }
}

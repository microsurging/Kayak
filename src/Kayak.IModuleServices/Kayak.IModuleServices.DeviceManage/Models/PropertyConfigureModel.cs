using Kayak.Core.Common.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kayak.IModuleServices.DeviceManage.Models
{
    public class PropertyConfigureModel
    {
        public int Id { get; set; }

        public string PropertyId { get; set; }

        public string PropertyName { get; set; }

        public string? DataTypeValue { get; set; }

        public int Precision { get; set; }
        public string SourceValue { get; set; }
        public string? DefaultValue { get; set; }

        public string? ValueRange { get; set; }

        public float? Step { get; set; }

        public string? UnitValue { get; set; }

        public int? MaxLength { get; set; } 
         
        public string ReadWrite
        {
            get;
            set;
        }
         
        public List<string> ReadWriteType {
            get;
            set;
        }

        public string Remark { get; set; }

        public string CorrelationId { get; set; }

        public string CorrelationFrom { get; set; }


        public DateTimeOffset? CreateDate { get; set; } = DateTimeOffset.Now;

        public DateTimeOffset? UpdateDate { get; set; } = DateTimeOffset.Now;
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kayak.DataAccess.DeviceData.Entities
{
    [Table("Physical_FunctionConfigure")]
    public class FunctionConfigure : EntityFull
    {
        public string FunctionId { get; set; }

        public string FunctionName { get; set; }

        public bool IsAsync { get; set; }

        public string? InputIds { get; set; }

        public string? OutputIds { get; set; }

        public string? Expands { get; set; }
        public string CorrelationId { get; set; }

        public string CorrelationFrom { get; set; }
    }
}

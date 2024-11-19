using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kayak.IModuleServices.DeviceManage.Models
{
    public class FunctionConfigureModel
    {
        public int Id { get; set; }
        public string FunctionId { get; set; }

        public string FunctionName { get; set; }

        public bool IsAsync { get; set; }

        public string? InputIds { get; set; }

        public string? OutputIds { get; set; }

        public string? Expands { get; set; }
        public string CorrelationId { get; set; }

        public string CorrelationFrom { get; set; }
         
        public string Remark { get; set; }

        public DateTimeOffset? CreateDate { get; set; } = DateTimeOffset.Now;

        public DateTimeOffset? UpdateDate { get; set; } = DateTimeOffset.Now;
    }
}

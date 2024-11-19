using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kayak.IModuleServices.ServiceManage.Model
{
    public class ModuleModel
    {
        public int Id { get; set; }
        public string ModuleCode { get; set; }
        public string ModuleName { get; set; }
        public string ModuleType { get; set; }
        public string? FileAddress { get; set; }
        public string Remark { get; set; }

        public string ModuleMode { get; set; }

        public DateTimeOffset? CreateDate { get; set; }

        public DateTimeOffset? UpdateDate { get; set; }
    }
}

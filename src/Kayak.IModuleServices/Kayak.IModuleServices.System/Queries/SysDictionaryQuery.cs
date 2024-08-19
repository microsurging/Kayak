using Kayak.Core.Common.BaseParam;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kayak.IModuleServices.System.Queries
{
    public class SysDictionaryQuery : BaseQuery
    {
        public string? Code { get; set; }
         
        public string? Name { get; set; }

        public string? ParentCode { get; set; }
    }
}

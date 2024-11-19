using Kayak.Core.Common.BaseParam;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kayak.IModuleServices.ServiceManage.Query
{
    public class BlackWhiteListQuery : BaseQuery
    {
        public int? Status { get; set; } 
        public string RoutePathPattern { get; set; }
    }
}


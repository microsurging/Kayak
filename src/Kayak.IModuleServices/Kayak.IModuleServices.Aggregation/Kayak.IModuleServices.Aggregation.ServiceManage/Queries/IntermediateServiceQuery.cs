using Kayak.Core.Common.BaseParam;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kayak.IModuleServices.Aggregation.ServiceManage.Queries
{
    public  class IntermediateServiceQuery : BaseQuery
    { 
        public string? ServiceId { get; set; }

        public string ModuleName { get; set; }

        public string ServiceType { get; set; } 

        public string RoutePath { get; set; }
    }
}


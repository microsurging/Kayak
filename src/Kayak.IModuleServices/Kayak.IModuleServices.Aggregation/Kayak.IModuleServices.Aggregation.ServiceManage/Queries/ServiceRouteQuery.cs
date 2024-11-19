using Kayak.Core.Common.BaseParam;
using Kayak.IModuleServices.Aggregation.ServiceManage.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kayak.IModuleServices.Aggregation.ServiceManage.Queries
{
    public class ServiceRouteQuery : BaseQuery
    {
        public StatisticsMode? Mode {  get; set; }

        public int? RegistryCenterType { get; set; }

        public string? serviceId { get; set; }
    }
}


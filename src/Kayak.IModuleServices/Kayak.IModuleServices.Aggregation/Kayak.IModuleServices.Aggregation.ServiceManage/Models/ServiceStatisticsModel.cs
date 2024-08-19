using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kayak.IModuleServices.Aggregation.ServiceManage.Models
{
    public class ServiceStatisticsModel
    {
        public int ServiceTotal { get; set; }

        public int ServiceRunCount { get; set; }

        public int ServiceAbnormalCount { get; set; }

        public int ServiceNodeCount { get; set; }
    }
}

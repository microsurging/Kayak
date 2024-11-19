using Kayak.IModuleServices.DeviceManage.Models;
using Surging.Core.System.Intercept;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kayak.IModuleServices.DeviceManage.Queries
{
    public class StatisticsQuery
    {
        [CacheKey(1)]
        public DateTime StartDate { get; set; }

        [CacheKey(2)]
        public  DateTime EndDate { get; set; }

        [CacheKey(3)]
        public GroupType? GroupType {  get; set; }
    }
}

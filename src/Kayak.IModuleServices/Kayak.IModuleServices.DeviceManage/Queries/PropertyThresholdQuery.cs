using Surging.Core.System.Intercept;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kayak.IModuleServices.DeviceManage.Queries
{
    public class PropertyThresholdQuery
    {
        [CacheKey(1)]
        public string PropertyCode {  get; set; }
        [CacheKey(2)]
        public string ProductCode { get; set; }
        [CacheKey(3)]
        public string DeviceCode { get; set; }
    }
}

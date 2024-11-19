using Surging.Core.System.Intercept;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kayak.IModuleServices.DeviceManage.Models
{
    public class EditPropertyThresholds
    {
        [CacheKey(1)]
        public int PropertyId { get; set; }

        public List<PropertyThresholdModel> PropertyThresholds { get; set; }
    }
}

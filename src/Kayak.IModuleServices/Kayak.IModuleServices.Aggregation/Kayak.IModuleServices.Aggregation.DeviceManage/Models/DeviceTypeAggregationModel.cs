using Kayak.IModuleServices.DeviceManage.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kayak.IModuleServices.Aggregation.DeviceManage.Models
{
    public class DeviceTypeAggregationModel: DeviceTypeModel
    {
        public DataDictionary? ProductCategory { get; set; }

        public DataDictionary? Organization { get; set; }

        public DataDictionary? Protocol { get; set; }
    }
}

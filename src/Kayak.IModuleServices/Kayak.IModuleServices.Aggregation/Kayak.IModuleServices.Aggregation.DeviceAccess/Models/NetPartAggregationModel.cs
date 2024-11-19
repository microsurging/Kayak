using Kayak.IModuleServices.DeviceAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kayak.IModuleServices.Aggregation.DeviceAccess.Models
{
    public  class NetPartAggregationModel: NetworkPartModel
    {
        public NetPartDictionary ComponentType { get; set; }

        public NetPartDictionary ClusterMode { get; set; }
    }
}

using Kayak.IModuleServices.DeviceManage.Models;
using Kayak.IModuleServices.System.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kayak.IModuleServices.Aggregation.DeviceManage.Models
{
    public class PrdAggregationModel: ProductModel
    { 
        public ProductCategoryModel Category { get; set; }
          
        public PrdDictionary PrdProtocol { get; set; }
         
        public PrdDictionary PrdDeviceType { get; set; }
         
    }
}

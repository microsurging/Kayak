using Kayak.IModuleServices.ServiceManage.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kayak.IModuleServices.Aggregation.ServiceManage.Models
{
    public class RegCenterAggregationModel : RegistryCenterModel
    {
        public RegCenterDictionary RegCenterType { get; set; }

        public string RegCenterTypeName
        {
            get
            {
                return RegCenterType?.Name??"";
            }

        }
    }
}

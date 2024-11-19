using Kayak.IModuleServices.DeviceManage.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kayak.IModuleServices.Aggregation.DeviceManage.Models
{
    public class PropConfigModel: PropertyConfigureModel
    {
          public DataTypeDictionary PropDataType { get; set; }
    }
}

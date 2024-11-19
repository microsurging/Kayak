using Surging.Core.System.Intercept;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kayak.IModuleServices.DeviceManage.Queries
{
    public class FunctionParameterQuery
    {
        public string FunctionCode { get; set; }

        public string ParameterType { get; set; }
 
        public string ProductCode { get; set; }

        public string DeviceCode { get; set; }
    }
}

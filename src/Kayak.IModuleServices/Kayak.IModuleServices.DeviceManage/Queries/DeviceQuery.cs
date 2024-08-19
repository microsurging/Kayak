using Kayak.Core.Common.BaseParam;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kayak.IModuleServices.DeviceManage.Queries
{
    public class DeviceQuery : BaseQuery
    {
        public string Code { get; set; }

        public string Name { get; set; }

        public List<string> ProductCodes { get; set; }
    }
}

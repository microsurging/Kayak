using Kayak.Core.Common.BaseParam;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kayak.IModuleServices.DeviceAccess.Queries
{
    public class NetworkPartQuery : BaseQuery
    {
        public string Name { get; set; }

        public string ComponentType { get; set; }
    }

}

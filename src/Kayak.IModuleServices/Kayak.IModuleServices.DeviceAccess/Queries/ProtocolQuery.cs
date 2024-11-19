using Kayak.Core.Common.BaseParam;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kayak.IModuleServices.DeviceAccess.Queries
{
    public class ProtocolQuery : BaseQuery
    {
        public int? Status {  get; set; }
        public string ProtocolName { get; set; }

    }
}

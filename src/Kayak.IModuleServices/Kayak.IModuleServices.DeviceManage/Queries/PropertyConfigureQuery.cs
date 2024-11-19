using Kayak.Core.Common.BaseParam;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kayak.IModuleServices.DeviceManage.Queries
{
    public class PropertyConfigureQuery : BaseQuery
    {
        public string InputText {  get; set; }

        public string CorrelationId { get; set; }

        public string CorrelationFrom { get; set; }
    }
}

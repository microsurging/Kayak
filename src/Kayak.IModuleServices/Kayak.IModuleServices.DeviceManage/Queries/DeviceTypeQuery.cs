using Kayak.Core.Common.BaseParam;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kayak.IModuleServices.DeviceManage.Queries
{
    public class DeviceTypeQuery : BaseQuery
    {
        public string DeviceTypeCode { get; set; }

        public int? OrganizationId { get; set; }

        public string? ProtocolCode { get; set; }
         
    }
}

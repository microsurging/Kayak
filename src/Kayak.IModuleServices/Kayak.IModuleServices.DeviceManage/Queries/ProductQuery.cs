using Kayak.Core.Common.BaseParam;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kayak.IModuleServices.DeviceManage.Queries
{
    public class ProductQuery : BaseQuery
    {
        public string ProductCode { get; set; }

        public int? OrganizationId { get; set; }

        public int? CategoryId { get; set; }

        public int? Protocol { get; set; }

        public int? DeviceType { get; set; }
    }
}

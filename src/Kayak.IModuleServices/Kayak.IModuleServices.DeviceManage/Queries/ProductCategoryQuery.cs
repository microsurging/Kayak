using Kayak.Core.Common.BaseParam;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kayak.IModuleServices.DeviceManage.Queries
{
    public class ProductCategoryQuery : BaseQuery
    {
        public string Name { get; set; }

        public string CategoryId { get; set; }

        public int? Level { get; set; }

        public string Code { get; set; }
    }
}

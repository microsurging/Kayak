using Kayak.Core.Common.BaseParam;
using Kayak.Core.Common.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kayak.IModuleServices.System.Queries
{
    public class SysOrganizationQuery:BaseQuery
    {
        public string Name { get; set; }

        public int? Level { get; set; }

        public int?  SysOrgType { get; set; }

        public string LevelCode { get; set; }
    }
}

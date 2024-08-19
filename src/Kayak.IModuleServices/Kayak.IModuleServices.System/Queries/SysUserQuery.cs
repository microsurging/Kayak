using Kayak.Core.Common.BaseParam;
using Kayak.Core.Common.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kayak.IModuleServices.System.Queries
{
    public class SysUserQuery : BaseQuery
    {
        public string Phone { get; set; }


        public string Email { get; set; }

        public UserGenderEnum? Sex { get; set; }


        public string UserName { get; set; }
    }
}
 
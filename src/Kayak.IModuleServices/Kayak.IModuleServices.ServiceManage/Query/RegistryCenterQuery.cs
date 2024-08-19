using Kayak.Core.Common.BaseParam;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kayak.IModuleServices.ServiceManage.Query
{
    public class RegistryCenterQuery : BaseQuery
    {
         public int? RegistryCenterType { get; set; }

         public string Name { get; set; }
    }
}

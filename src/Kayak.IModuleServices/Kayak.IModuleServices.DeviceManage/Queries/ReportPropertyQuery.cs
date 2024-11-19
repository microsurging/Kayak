using Kayak.Core.Common.BaseParam;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kayak.IModuleServices.DeviceManage.Queries
{
    public class ReportPropertyQuery : BaseQuery
    {
        public string DeviceId {  get; set; }

        public string? PropertyId { get; set; }

        public string? PropertyName { get; set; }

         public DateTime? BeginDate { get; set; }

        public DateTime? EndDate { get;set; }
    }
}

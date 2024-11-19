using Kayak.Core.Common.BaseParam;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kayak.IModuleServices.System.Queries
{
    public class ReportPropertyLogQuery : BaseQuery
    {

        public int? PropertyId {  get; set; }
        public string PropertyName {  get; set; }
        public string ProductCode { get; set; }
        public string DeviceCode { get; set; }

        public string Level { get; set; }

        public DateTimeOffset? StartTime { get; set; }

        public DateTimeOffset? EndTime { get; set; }
    }
}

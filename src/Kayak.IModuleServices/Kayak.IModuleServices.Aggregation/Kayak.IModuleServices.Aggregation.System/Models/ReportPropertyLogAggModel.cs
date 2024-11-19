using Kayak.IModuleServices.System.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kayak.IModuleServices.Aggregation.System.Models
{
    public class ReportPropertyLogAggModel: ReportPropertyLogModel
    {
        public string PropertyName { get; set; }

        public string Duration { get; set; }

        public string Reason { get; set; }


    }
}

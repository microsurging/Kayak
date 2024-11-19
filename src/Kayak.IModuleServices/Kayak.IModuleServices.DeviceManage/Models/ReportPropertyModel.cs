using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kayak.IModuleServices.DeviceManage.Models
{
    public class ReportPropertyModel
    {
        public int Id { get; set; }

        public string DeviceId { get; set; }

        public string PropertyId { get; set; }

        public string PropertyValue { get; set; }

       public DateTimeOffset? CreateDate { get; set; } = DateTimeOffset.Now;
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kayak.IModuleServices.DeviceManage.Models
{
    public class DeviceEventModel
    {
        public int Id { get; set; }
        public string DeviceId { get; set; }

        public string EventId { get; set; }

        public string EventOutParams { get; set; }

        public string EventOutParamValues { get; set; }
        public DateTimeOffset? CreateDate { get; set; } = DateTimeOffset.Now;
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kayak.IModuleServices.DeviceReport.Models
{
    public class WillMessage
    {
        public string Topic { get; set; }

        public string Message { get; set; }


        public bool WillRetain { get; set; }

        public int Qos { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kayak.IModuleServices.DeviceAccess.Models
{
    public class NetworkPartModel
    {
        public string Name { get; set; }

        public string ComponentType { get; set; }

        public string ClusterMode { get; set; }

        public string Status { get; set; }

        public bool EnableSSL { get; set; }

        public string RuleScript { get; set; }

        public string Host { get; set; }

        public int Port { get; set; }

        public  DateTime? CreateDate { get; set; }

        public DateTime? UpdateDate { get; set; }   

        public string Remark { get; set; }
    }
}

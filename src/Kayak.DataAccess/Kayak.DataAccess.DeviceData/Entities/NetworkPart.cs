using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kayak.DataAccess.DeviceData.Entities
{
    [Table("Component_NetworkPart")]
    public class NetworkPart : EntityFull
    {
        public string Name { get; set; }

        public string ComponentType { get; set; }

        public string ClusterMode { get; set; }

        public string Status { get; set; }

        public bool EnableSSL {  get; set; }

        public string RuleScript { get; set; }

        public string Host {  get; set; }

        public int Port { get; set; }

    }
}

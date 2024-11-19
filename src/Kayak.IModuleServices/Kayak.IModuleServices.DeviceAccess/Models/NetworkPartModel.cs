using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kayak.IModuleServices.DeviceAccess.Models
{
    public class NetworkPartModel
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public string ComponentTypeCode { get; set; }

        public int ClusterModeId { get; set; }

        public int Status { get; set; }

        public bool EnableSSL { get; set; }

        public bool? EnableTLS { get; set; }

        public bool? EnableSwagger { get; set; }

        public bool? EnableWebService { get; set; }

        public bool? IsMulticast { get; set; }

        public int? ResolveMode { get; set; }

        public int? FixedLength { get; set; }

        public string? Delimited { get; set; }

        public int? MaxMessageLength { get; set; }

        public string RuleScript { get; set; }

        public string Host { get; set; }

        public int Port { get; set; }

        public DateTimeOffset? CreateDate { get; set; } = DateTimeOffset.Now;

        public DateTimeOffset? UpdateDate { get; set; } = DateTimeOffset.Now;

        public string Remark { get; set; }
    }
}

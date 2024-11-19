using Microsoft.Extensions.Logging;
using Surging.Core.CPlatform.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Kayak.IModuleServices.System.Models
{
    public class NetworkLogModel
    {
        public int Id { get; set; } 
        public string NetworkId { get; set; }

        [JsonConverter(typeof(JsonStringEnumConverter))]
        public LogLevel logLevel { get; set; }

        public string logLevelName { get; set; }

        public NetworkType NetworkType { get; set; }

        public string NetworkTypeName { get; set; }

        public string EventName { get; set; }

        public string Content { get; set; }

        public DateTimeOffset? CreateDate { get; set; } = DateTimeOffset.Now;
    }
}

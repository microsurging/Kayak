using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Surging.Core.CPlatform;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Kayak.IModuleServices.Aggregation.ServiceManage.Models
{
    public class ServiceEntryModel
    {
        public string ModuleName { get; set; }
        public IEnumerable<string> Methods { get; set; }
        public string RoutePath { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        public ServiceType ServiceType { get;set; }
        public bool IsPermission { get; set; }
         
        public string MethodName { get; set; }

        /// <summary>
        /// 服务描述符。
        /// </summary>
        public ServiceDescriptor Descriptor { get; set; }
    }
}

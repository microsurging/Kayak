using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kayak.Core.Common.Intercepte.Model
{
    public class OperateLogModel
    {
        public int Id { get; set; } 
       public string ServiceId { get; set; }

        public string RoutePath { get; set; }

       public string  Arguments { get; set; }

        public string ReturnType { get; set; }

        public string? Payload { get; set; }

       public   string  ReturnValue { get; set; }

        public  long? RunTime { get; set; }

        public DateTimeOffset? CreateDate { get; set; }
    }
}

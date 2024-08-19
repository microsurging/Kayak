using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kayak.Core.Common.Intercepte
{
    internal class InterceptorOptions
    {
        public InterceptorOptions() { }

        public bool DisableCache { get; set; }

        public bool DisableOperateLog { get; set; }
    }
}

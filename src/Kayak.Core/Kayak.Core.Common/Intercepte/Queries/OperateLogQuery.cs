﻿using Kayak.Core.Common.BaseParam;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kayak.Core.Common.Intercepte.Queries
{
    public class OperateLogQuery : BaseQuery
    {
        public string RoutePath { get; set; }

        public DateTime? StartTime { get; set; }

        public DateTime? EndTime { get; set;}
    }
}
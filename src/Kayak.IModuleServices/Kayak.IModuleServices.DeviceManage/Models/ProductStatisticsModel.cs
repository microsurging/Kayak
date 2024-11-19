﻿using Microsoft.EntityFrameworkCore.Metadata;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kayak.IModuleServices.DeviceManage.Models
{
    public class ProductStatisticsModel
    {
        public int NormalCount { get; set; }

        public int DisableCount {  get; set; }
    }
}
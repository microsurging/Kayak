using Surging.Core.CPlatform.Configurations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kayak.DataAccess.DeviceData
{
    public class AppConfig
    {
        private static DeviceDataOption _serverOptions = new DeviceDataOption();
        public static  DeviceDataOption? DeviceDataOptions
        {
            get
            {
                return _serverOptions;
            }
            set
            {
                _serverOptions = value;
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kayak.DataAccess.DeviceData
{
    public class DeviceDataOption
    {
        public string Name { get; set; }

        public DatabaseType DatabaseType { get; set; }

        public string Connstring { get; set; }
    }
}

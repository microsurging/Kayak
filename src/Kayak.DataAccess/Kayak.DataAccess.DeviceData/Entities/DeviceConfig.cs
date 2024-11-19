using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kayak.DataAccess.DeviceData.Entities
{
    [Table("Component_DeviceConfig")]
    public class DeviceConfig : EntityFull
    {
        public string ProductCode { get; set; }

        public string DeviceCode { get; set; }

        public string AuthConfig { get; set; }
    }
}

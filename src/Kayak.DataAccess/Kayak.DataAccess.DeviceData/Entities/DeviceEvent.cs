using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kayak.DataAccess.DeviceData.Entities
{
    [Table("Device_Event")]
    public class DeviceEvent : EntityFull
    {
        public string DeviceId { get; set; }

        public string EventId { get; set; }

        public string EventOutParams{ get; set; }

        public string EventOutParamValues { get; set; }
    }
}

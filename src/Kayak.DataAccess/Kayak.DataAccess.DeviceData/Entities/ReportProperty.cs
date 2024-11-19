using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kayak.DataAccess.DeviceData.Entities
{
    [Table("Device_ReportProperty")]
    public class ReportProperty : EntityFull
    {
        public string DeviceId { get; set; }

        public string PropertyId { get; set; }

        public string PropertyValue { get; set; }
    }
}

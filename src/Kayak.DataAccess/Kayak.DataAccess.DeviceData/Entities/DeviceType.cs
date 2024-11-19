using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kayak.DataAccess.DeviceData.Entities
{
    [Table("Component_DeviceType")]
    public class DeviceType:EntityFull
    { 

        public string Code { get; set; }
        public string DeviceTypeName { get; set; }

        public int? ProductCategoryId { get; set; }


        public int? OrganizationId { get; set; }


        public string? ConnProtocolCode { get; set; }


        public string? ProtocolCode { get; set;}


        public string DeviceTypeCode { get; set; }
    }
}

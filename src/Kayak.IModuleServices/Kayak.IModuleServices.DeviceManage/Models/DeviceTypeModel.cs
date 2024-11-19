using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kayak.IModuleServices.DeviceManage.Models
{
    public class DeviceTypeModel
    {
        public int Id { get; set; } 

        public string Code { get; set; }

        public string DeviceTypeName { get; set; }

        public int? ProductCategoryId { get; set; }


        public int? OrganizationId { get; set; }


        public string? ConnProtocolCode { get; set; }


        public string? ProtocolCode { get; set; }


        public string DeviceTypeCode { get; set; }

        public string Remark { get; set; }


        public DateTimeOffset? UpdateDate { get; set; } = DateTimeOffset.Now;

        public DateTimeOffset? CreateDate { get; set; } =  DateTimeOffset.Now;
    }
}

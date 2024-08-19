using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kayak.IModuleServices.DeviceManage.Models
{
    public class ProductModel
    {
        public int Id { get; set; }
        public string ProductCode { get; set; }

        public string ProductName { get; set; }

        public int OrganizationId { get; set; }

        public int CategoryId { get; set; }

        public bool IsDeleted { get; set; } = false;
        public int Protocol { get; set; }

        public int DeviceType { get; set; }

        public int Status { get;set; }


        public string Remark { get; set; }

        public DateTime? UpdateDate { get; set; }

        public DateTime? CreateDate { get; set; }
    }
}

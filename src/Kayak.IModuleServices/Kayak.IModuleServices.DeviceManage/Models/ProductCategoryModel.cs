using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kayak.IModuleServices.DeviceManage.Models
{
    public class ProductCategoryModel
    {
        public int Id { get; set; }
        public string CategoryId { get; set; }

        public string CategoryName { get; set; }
        public bool IsChildren { get; set; }
        public int Level { get; set; }

        public string Code { get; set; }

        public DateTime? CreateDate { get; set; }=DateTime.Now;

        public DateTime? UpdateDate { get; set; } = DateTime.Now;
        public string Remark { get; set; }
    }
}

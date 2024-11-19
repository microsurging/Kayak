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

        public DateTimeOffset? CreateDate { get; set; }= DateTimeOffset.Now;

        public DateTimeOffset? UpdateDate { get; set; } = DateTimeOffset.Now;
        public string Remark { get; set; }
    }
}

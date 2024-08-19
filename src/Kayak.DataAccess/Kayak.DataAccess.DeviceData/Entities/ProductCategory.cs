using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kayak.DataAccess.DeviceData.Entities
{
    [Table("Component_ProductCategory")]
    public class ProductCategory : EntityFull
    {
        public string CategoryId { get; set; }

        public string CategoryName { get; set; }

        public int Level { get; set; }

        public string Code { get; set; }

        public bool IsChildren { get; set; }
    }
}

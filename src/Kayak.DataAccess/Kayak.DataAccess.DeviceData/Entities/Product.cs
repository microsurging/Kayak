using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kayak.DataAccess.DeviceData.Entities
{
    [Table("Component_Product")]
    public class Product : EntityFull
    {
        public string ProductCode { get; set; }

        public string ProductName { get; set; }

        public int OrganizationId { get; set; }

        public int? CategoryId { get; set; }

        public int Protocol { get; set; }

        public int DeviceType { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        [Description("状态")]
        [Column("State")]
        public int Status { get; set; } = 0;


    }
}

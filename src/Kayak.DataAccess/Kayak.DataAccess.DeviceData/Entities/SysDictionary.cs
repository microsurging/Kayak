using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kayak.DataAccess.DeviceData.Entities
{
    [Table("Sys_Dictionary")]
    public class SysDictionary : EntityFull
    {
        /// <summary>
        /// 名称
        /// </summary>
        [Description("名称")]
        [Column("Name", TypeName = "varchar(50)")]
        public string Name { get; set; }

        /// <summary>
        /// 编码
        /// </summary>
        [Description("编码")]
        [Column("Code", TypeName = "varchar(50)")]
        public string Code { get; set; }

        /// <summary>
        /// 数值
        /// </summary>
        [Description("数值")]
        [Column("Value")]
        public int? Value { get; set; }

        /// <summary>
        /// 父级Code
        /// </summary>
        [Description("父级Code")]
        [Column("ParentCode", TypeName = "varchar(50)")]
        public string? ParentCode { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        [Description("状态")]
        [Column("State")]
        public int Status { get; set; } = 1;
        /// <summary>
        /// 是否显示
        /// </summary>
        [Description("是否显示")]
        [Column("IsShow")]
        public int IsShow { get; set; }
    }
}


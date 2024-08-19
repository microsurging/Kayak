using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Kayak.Core.Common.Repsitories.Entities;

namespace Kayak.DataAccess.DeviceData.Entities
{
    public abstract class EntityFull: ObjectEntity
    { 
        /// <summary>
        /// 创建人
        /// </summary>
        public int? Creater { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime? CreateDate { get; set; }

        /// <summary>
        /// 修改人
        /// </summary>
        public int? Updater { get; set; }

        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime? UpdateDate { get; set; }

        /// <summary>
        /// 删除
        /// </summary>
        public bool IsDeleted { get; set; } = false;

        /// <summary>
        /// 备注
        /// </summary>
        [Description("备注")]
        [Column("Remark")]
        public string? Remark { get; set; }
    }
}

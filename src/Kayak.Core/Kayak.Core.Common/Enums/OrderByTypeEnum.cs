using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kayak.Core.Common.Enums
{
    public enum OrderByTypeEnum
    {
        /// <summary>
        /// 升序
        /// </summary>
        [Display(Name = "升序")]
        Asc,
        /// <summary>
        /// 降序
        /// </summary>
        [Display(Name = "降序")]
        Desc
    }
}

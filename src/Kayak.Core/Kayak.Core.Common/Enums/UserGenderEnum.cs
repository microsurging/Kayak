using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kayak.Core.Common.Enums
{
    public enum UserGenderEnum
    {
        /// <summary>
        /// 男
        /// </summary>
        [Display(Name = "男")]
        Man,
        /// <summary>
        /// 女
        /// </summary>
        [Display(Name = "女")]
        Woman
    }
}

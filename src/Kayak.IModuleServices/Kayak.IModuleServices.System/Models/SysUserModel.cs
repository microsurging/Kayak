using Kayak.Core.Common.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kayak.IModuleServices.System.Models
{
    public class SysUserModel
    {
        public int UserId { get; set; }
        public string UserName { get; set; }

        public UserGenderEnum Sex { get; set; }
        public string PhoneNumber { get; set; }

        public string Password { get; set; }
         
        public string? Avatar { get; set; }
         
        public string? RealName { get; set; }
        public string Phone { get; set; }
        public string? Email { get; set; }

        public string? QQToken { get; set; }

        public string? WeChatToken { get; set; }

        public string? Remark { get; set; }
        public int Status { get; set; } = 1;

        public DateTime? CreateTime { get; set; }
        public DateTime? UpdateTime { get; set; }
    }
}

using Kayak.Core.Common.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kayak.IModuleServices.System.Models
{
    public class UserInfo
    {
        public int UserId { get; set; }
        public string UserName { get; set; }

        public UserGenderEnum Sex { get; set; }
        public string PhoneNumber { get; set; }
         
        public string? Avatar { get; set; }

        public string? RealName { get; set; }
        public string Phone { get; set; }
        public string? Email { get; set; }

        public DateTime CreateDate { get; set; }
    }
}

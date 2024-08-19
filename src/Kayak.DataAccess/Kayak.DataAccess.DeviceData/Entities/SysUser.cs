using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace Kayak.DataAccess.DeviceData.Entities
{
    [Table("Sys_User")]
    public class SysUser: EntityFull
    {

        [MaxLength(50)]
        [Required]
        public string UserName { get; set; }

        [MaxLength(20)] 
        public string PhoneNumber { get; set; }


        public int Sex { get; set; }

        [MaxLength(20)]
        public string Password { get; set; }

        [MaxLength(50)]
        public string? Avatar { get; set; }

        [MaxLength(20)]
        public string? RealName { get; set; }

        [MaxLength(20)]
        [Required]
        public string Phone { get; set; }

        public string? Email { get; set; }


        [MaxLength(20)]
        public string? QQToken { get; set; }


        [MaxLength(20)]
        public string? WeChatToken { get; set; }

        [MaxLength(20)]
        public string? AlipayToken { get; set; }

        [MaxLength(50)]
        public string? QRCode { get; set; }


        public int Status { get; set; } = 1;



    }
}

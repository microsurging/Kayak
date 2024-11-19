using Kayak.Core.Common.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kayak.IModuleServices.System.Models
{
    public class SysOrganizationModel
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public string LevelCode { get; set; }

        public int Level { get; set; }

        public string City { get; set; }

        public string Address { get; set; }

        public string Phone { get; set; }

        public string Email { get; set; }

        public string Contacter { get; set; }

        public DateTimeOffset? CreateDate { get; set; }= DateTimeOffset.Now;

        public DateTimeOffset? UpdateDate { get; set; } = DateTimeOffset.Now;

        public int SysOrgType { get; set; }

        public string Remark { get; set; }

        public int Status { get; set; }
    }
}

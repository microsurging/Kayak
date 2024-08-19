using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kayak.DataAccess.DeviceData.Entities
{
    [Table("Sys_Organization")]
    public class SysOrganization: EntityFull
    {
        public string Name { get; set; }

        public string LevelCode { get; set; }

        public int Level { get; set; }

        public string City { get; set; }

        public string Address { get; set; }

        public string Phone { get; set; }

        public string Email { get; set; }

        public string Contacter { get; set; }

        public int SysOrgType { get; set; }

        public int Status { get; set; }
    }
}

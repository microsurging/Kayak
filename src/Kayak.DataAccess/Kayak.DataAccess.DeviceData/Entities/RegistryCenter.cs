using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kayak.DataAccess.DeviceData.Entities
{
    [Table("Component_RegistryCenter")]
    public  class RegistryCenter : EntityFull
    {
        public string Name {  get; set; }   

        public int RegistryCenterType { get; set; }

        public string Path { get; set; }

        public string Host { get; set; }

        public int Port { get; set; }   

    }
}

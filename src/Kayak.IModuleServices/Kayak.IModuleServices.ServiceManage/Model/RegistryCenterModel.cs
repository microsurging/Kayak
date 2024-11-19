using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kayak.IModuleServices.ServiceManage.Model
{
    public class RegistryCenterModel
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public int RegistryCenterType { get; set; }

        public string Path { get; set; }

        public string Host { get; set; }

        public int Port { get; set; }

        public string Remark { get; set; }

        public DateTimeOffset? CreateDate { get; set; }
    }
}

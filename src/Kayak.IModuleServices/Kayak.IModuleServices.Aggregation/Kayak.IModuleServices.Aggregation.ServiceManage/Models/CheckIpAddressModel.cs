using Surging.Core.CPlatform.Address;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kayak.IModuleServices.Aggregation.ServiceManage.Models
{
    public class CheckIpAddressModel
    {
        public IpAddressModel Address { get; set; }

        public bool IsHealth { get; set; }
    }
}

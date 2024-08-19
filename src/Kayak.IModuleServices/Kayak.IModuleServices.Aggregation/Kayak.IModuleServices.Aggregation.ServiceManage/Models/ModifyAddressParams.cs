using Surging.Core.CPlatform.Address;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Kayak.IModuleServices.Aggregation.ServiceManage.Models
{
    public class ModifyAddressParams : AddressModel
    {
        public string Ip { get; set; }

        public string ServiceId { get; set; }
        public int? RegistryCenterType { get; set; }
        public int Port { get; set; }

        public override EndPoint CreateEndPoint()
        {
            return new IPEndPoint(IPAddress.Parse(Ip), Port);
        }

        public override string ToString()
        {
            return string.Concat(new string[] { Ip, ":", Port.ToString() });
        }

    }
}

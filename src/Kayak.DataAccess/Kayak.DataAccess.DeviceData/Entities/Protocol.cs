using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kayak.DataAccess.DeviceData.Entities
{
    [Table("Component_Protocol")]
    public class Protocol : EntityFull
    {
        public string ProtocolCode { get; set; }

        public string ProtocolName { get; set; }

        public string ProtocolType { get; set; }

        public string? Script { get; set; }

        public string? ConnProtocol {  get; set; }

        public int Status {  get; set; }    

        public string? ClassName { get; set; }

        public string? FileAddress { get; set; }
    }
}

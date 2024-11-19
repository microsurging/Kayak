using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kayak.DataAccess.DeviceData.Entities
{
    [Table("Physical_FunctionParameter")]
    public class FunctionParameter : EntityFull
    {
        public int FunctionId { get; set; }

        public string FunctionCode { get; set; }

        public string? DeviceCode { get; set; }

        public string ProductCode { get; set; }

        public string ParameterType { get;set; }

        public string Code { get; set; }

        public string Name { get;set; }

        public string? Constraint { get; set; }

        public string DataTypeValue { get; set; }

    }
}

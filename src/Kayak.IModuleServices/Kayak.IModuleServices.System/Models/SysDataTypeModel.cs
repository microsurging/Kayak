using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kayak.IModuleServices.System.Models
{
    public class SysDataTypeModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int? Value { get; set; }

        public string DefaultValue { get; set; }

        public string Remark { get; set; }

        public DateTimeOffset? CreateDate { get; set; } = DateTimeOffset.Now;
        public DateTimeOffset? UpdateDate { get; set; } = DateTimeOffset.Now;
    }
}

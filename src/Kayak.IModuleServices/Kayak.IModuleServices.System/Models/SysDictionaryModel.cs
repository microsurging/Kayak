using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kayak.IModuleServices.System.Models
{
    public  class SysDictionaryModel
    { 
        public int Id { get; set; }
        public string Name { get; set; }
         
        public string Code { get; set; }
         
        public int? Value { get; set; }
         
        public string? ParentCode { get; set; }

        public int Status { get; set; } = 1;

        public int IsShow { get; set; }

        public DateTime? CreateDate { get; set; }=DateTime.Now;

        public DateTime? UpdateDate { get; set; } =DateTime.Now;

        public string? Remark { get; set; }
    }
}

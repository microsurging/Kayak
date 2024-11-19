using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Surging.Core.System.Intercept;

namespace Kayak.IModuleServices.System.Models
{
    public  class SysDictionaryModel
    { 
        public int Id { get; set; }
        public string Name { get; set; }
         
        public string Code { get; set; }
         
        public int? Value { get; set; }

        [CacheKey(1)]
        public string? ParentCode { get; set; }

        public int Status { get; set; } = 1;

        public int IsShow { get; set; }

        public DateTimeOffset? CreateDate { get; set; }= DateTimeOffset.Now;

        public DateTimeOffset? UpdateDate { get; set; } = DateTimeOffset.Now;

        public string? Remark { get; set; }
    }
}

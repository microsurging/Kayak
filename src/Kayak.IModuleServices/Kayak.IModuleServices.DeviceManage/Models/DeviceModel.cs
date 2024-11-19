using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kayak.IModuleServices.DeviceManage.Models
{
    public class DeviceModel
    {
        public int Id { get; set; }
        public string Code { get; set; }

        public string Name { get; set; }

        public string ProductCode { get; set; }

         
        public int Status { get; set; } = 1;

        public bool IsDeleted { get; set; }

        public string Remark { get; set; }


        public DateTimeOffset? UpdateDate { get; set; }= DateTimeOffset.Now;

        public DateTimeOffset? CreateDate { get; set; }=DateTimeOffset.Now;
    }
}

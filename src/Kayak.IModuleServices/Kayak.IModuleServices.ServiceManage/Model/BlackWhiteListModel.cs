using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kayak.IModuleServices.ServiceManage.Model
{
    public class BlackWhiteListModel
    {
        public int Id { get; set; }
        public string RoutePathPattern { get; set; }

        public string BlackList { get; set; }

        public string WhiteList { get; set; }

        public int Status { get; set; }

        public string Remark { get; set; }

        public DateTimeOffset? CreateDate { get; set; }

        public DateTimeOffset? UpdateDate { get; set; }
    }
}

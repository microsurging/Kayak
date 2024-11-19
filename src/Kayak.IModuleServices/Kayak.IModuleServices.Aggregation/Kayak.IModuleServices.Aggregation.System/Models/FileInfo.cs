using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kayak.IModuleServices.Aggregation.System.Models
{
    public class UploadFileInfo
    {
        public string Id { get; set; }=Guid.NewGuid().ToString("N");

        public string Name { get; set; }

        public long Length { get; set; }

        public DateTimeOffset? CreateDate { get; set; }= DateTimeOffset.Now;
    }
}

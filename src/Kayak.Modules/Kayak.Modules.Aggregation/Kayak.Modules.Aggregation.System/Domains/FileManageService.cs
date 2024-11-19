using Kayak.Core.Common.Response;
using Kayak.IModuleServices.Aggregation.System;
using Kayak.IModuleServices.Aggregation.System.Models;
using Surging.Core.CPlatform.Ioc;
using Surging.Core.KestrelHttpServer.Internal;
using Surging.Core.ProxyGenerator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kayak.Modules.Aggregation.System.Domains
{
    internal class FileManageService : ProxyServiceBase, IFileManageService, ISingleInstance
    {
        public async Task<ApiResult<List<UploadFileInfo>>> UploadFile(HttpFormCollection form1)
        {
            var files = form1.Files;
            var list=new List<UploadFileInfo>();
            foreach (var file in files)
            {      
                var fileInfo = new UploadFileInfo();
                var path = Path.Combine(AppContext.BaseDirectory, fileInfo.Id);
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                using (var stream = new FileStream(Path.Combine(AppContext.BaseDirectory, fileInfo.Id, file.FileName), FileMode.OpenOrCreate))
                {
                    await stream.WriteAsync(file.File, 0, (int)file.Length);
                }
                fileInfo.Length = file.Length;
                fileInfo.Name= file.FileName;
                list.Add(fileInfo);
            }
            
            return ApiResult<List<UploadFileInfo>>.Succeed(list);
        }
    }
}

using Kayak.Core.Common.Response;
using Kayak.IModuleServices.Aggregation.System.Models;
using Surging.Core.CPlatform.Filters.Implementation;
using Surging.Core.CPlatform.Ioc;
using Surging.Core.CPlatform.Runtime.Server.Implementation.ServiceDiscovery.Attributes;
using Surging.Core.KestrelHttpServer.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kayak.IModuleServices.Aggregation.System
{
    [ServiceBundle("api/{Service}/{Method}")]
    public interface IFileManageService:IServiceKey
    {
        //[Authorization(AuthType = AuthorizationType.JWT)]
        Task<ApiResult<List<UploadFileInfo>>> UploadFile(HttpFormCollection form1);
    }
}

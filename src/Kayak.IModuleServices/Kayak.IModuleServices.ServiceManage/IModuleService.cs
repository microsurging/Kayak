using Kayak.Core.Common.Response;
using Kayak.IModuleServices.ServiceManage.Model;
using Kayak.IModuleServices.ServiceManage.Query;
using Surging.Core.CPlatform.Filters.Implementation;
using Surging.Core.CPlatform.Ioc;
using Surging.Core.CPlatform.Runtime.Server.Implementation.ServiceDiscovery.Attributes;
using Surging.Core.ProxyGenerator.Interceptors.Implementation.Metadatas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kayak.IModuleServices.ServiceManage
{
    [ServiceBundle("api/{Service}/{Method}")]
    public interface IModuleService : IServiceKey
    {
        [Authorization(AuthType = AuthorizationType.JWT)]
        Task<ApiResult<bool>> Add(ModuleModel model);

        [Authorization(AuthType = AuthorizationType.JWT)]      
        Task<ApiResult<bool>> Modify(ModuleModel model);

        [Authorization(AuthType = AuthorizationType.JWT)]
        Task<ApiResult<Page<ModuleModel>>> GetPageAsync(ModuleQuery query);

    }
}

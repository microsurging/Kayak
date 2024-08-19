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
    [ServiceBundle("api/{Service}/{Method}",isPrefix:true)]
    public interface IRegistryCenterService : IServiceKey
    {
        [Authorization(AuthType = AuthorizationType.JWT)] 
        [ServiceRoute("query")]
        Task<ApiResult<Page<RegistryCenterModel>>> GetPageAsync(RegistryCenterQuery query);
    

        [Authorization(AuthType = AuthorizationType.JWT)]
        [ServiceCacheIntercept(CachingMethod.Remove, "GetRegCenterList", CacheSectionType = "ddlCache", Mode = CacheTargetType.MemoryCache)]
        Task<ApiResult<bool>> Add(RegistryCenterModel model);

        [Authorization(AuthType = AuthorizationType.JWT)]
        [ServiceCacheIntercept(CachingMethod.Remove, "GetRegCenterList", CacheSectionType = "ddlCache", Mode = CacheTargetType.MemoryCache)]
        Task<ApiResult<bool>> DeleteById(List<int> ids);

        [Authorization(AuthType = AuthorizationType.JWT)]
        [ServiceCacheIntercept(CachingMethod.Remove, "GetRegCenterList", CacheSectionType = "ddlCache", Mode = CacheTargetType.MemoryCache)]
        Task<ApiResult<bool>> Modify(RegistryCenterModel model);

        [Authorization(AuthType = AuthorizationType.JWT)]
        [ServiceCacheIntercept(CachingMethod.Get, Key = "GetRegCenterList", CacheSectionType = "ddlCache", EnableL2Cache = false, Mode = CacheTargetType.MemoryCache, Time = 480, EnableStageCache = true)]
        Task<ApiResult<List<RegistryCenterModel>>> GetList();
    }
}

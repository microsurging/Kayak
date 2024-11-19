using Kayak.Core.Common.Response;
using Kayak.IModuleServices.DeviceAccess.Models;
using Kayak.IModuleServices.DeviceAccess.Queries;
using Surging.Core.CPlatform.Filters.Implementation;
using Surging.Core.CPlatform.Ioc;
using Surging.Core.CPlatform.Runtime.Server.Implementation.ServiceDiscovery.Attributes;
using Surging.Core.ProxyGenerator.Interceptors.Implementation.Metadatas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kayak.IModuleServices.DeviceAccess
{
    [ServiceBundle("api/{Service}/{Method}")]
    public interface IProtocolManageService : IServiceKey
    {
        [Authorization(AuthType = AuthorizationType.JWT)]
        [ServiceCacheIntercept(CachingMethod.Remove, "GetProtocols", CacheSectionType = "ddlCache", Mode = CacheTargetType.MemoryCache, EnableStageCache = true)]
        Task<ApiResult<bool>> Add(ProtocolModel model);

        [Authorization(AuthType = AuthorizationType.JWT)]
        [ServiceCacheIntercept(CachingMethod.Remove, "GetProtocols", CacheSectionType = "ddlCache", Mode = CacheTargetType.MemoryCache, EnableStageCache = true)]
        Task<ApiResult<bool>> Modify(ProtocolModel model);

        [Authorization(AuthType = AuthorizationType.JWT)]
        Task<ApiResult<ProtocolModel>> GetProtocol(int id);

        [Authorization(AuthType = AuthorizationType.JWT)]
        Task<ApiResult<Page<ProtocolModel>>> GetPageAsync(ProtocolQuery query);

        [Authorization(AuthType = AuthorizationType.JWT)]
        Task<ApiResult<ProtocolModel>> GetProtocolByCode(string code);

        [Authorization(AuthType = AuthorizationType.JWT)]
        [ServiceCacheIntercept(CachingMethod.Remove, "GetProtocols", CacheSectionType = "ddlCache", Mode = CacheTargetType.MemoryCache, EnableStageCache = true)]
        Task<ApiResult<bool>> CancelPublish(List<int> ids);

        [Authorization(AuthType = AuthorizationType.JWT)]
        [ServiceCacheIntercept(CachingMethod.Remove, "GetProtocols", CacheSectionType = "ddlCache", Mode = CacheTargetType.MemoryCache, EnableStageCache = true)]
        Task<ApiResult<bool>> Republish(List<int> ids);

        [Authorization(AuthType = AuthorizationType.JWT)]
        [ServiceCacheIntercept(CachingMethod.Remove, "GetProtocols", CacheSectionType = "ddlCache", Mode = CacheTargetType.MemoryCache, EnableStageCache = true)]
        Task<ApiResult<bool>> DeleteById(List<int> ids);
       
        [Authorization(AuthType = AuthorizationType.JWT)]
        [ServiceCacheIntercept(CachingMethod.Get, Key = "GetProtocols", CacheSectionType = "ddlCache", EnableL2Cache = false, Mode = CacheTargetType.MemoryCache, Time = 480, EnableStageCache = true)]
        Task<ApiResult<List<ProtocolModel>>> GetProtocols();
    }
}

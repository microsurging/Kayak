using Kayak.Core.Common.Response;
using Kayak.IModuleServices.System.Models;
using Kayak.IModuleServices.System.Queries;
using Surging.Core.CPlatform.Filters.Implementation;
using Surging.Core.CPlatform.Ioc;
using Surging.Core.CPlatform.Runtime.Server.Implementation.ServiceDiscovery.Attributes;
using Surging.Core.ProxyGenerator.Interceptors.Implementation.Metadatas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kayak.IModuleServices.System
{
    [ServiceBundle("api/{Service}/{Method}")]
    public interface ISysDictionaryService : IServiceKey
    {
        [Authorization(AuthType = AuthorizationType.JWT)]
        [ServiceLogIntercept]
        [ServiceCacheIntercept(CachingMethod.Remove, "GetSysDictionaryByParentCode_{0}", CacheSectionType = "ddlCache", Mode = CacheTargetType.MemoryCache)]
        Task<ApiResult<bool>>  Add(SysDictionaryModel model);

        [Authorization(AuthType = AuthorizationType.JWT)]
        [ServiceLogIntercept]
        Task<ApiResult<SysDictionaryModel>> GetSysDictionary(string code);

        [Authorization(AuthType = AuthorizationType.JWT)]
        [ServiceLogIntercept]
        Task<ApiResult<Page<SysDictionaryModel>>> GetPageAsync(SysDictionaryQuery query);

        [Authorization(AuthType = AuthorizationType.JWT)]
        [ServiceCacheIntercept(CachingMethod.Get, Key = "GetSysDictionaryByParentCode_{0}", CacheSectionType = "ddlCache", EnableL2Cache = false, Mode = CacheTargetType.MemoryCache, Time = 10, EnableStageCache = true)]
        [ServiceLogIntercept]
        Task<ApiResult<List<SysDictionaryModel>>> GetSysDictionaryByParentCode(string parentCode);

        [Authorization(AuthType = AuthorizationType.JWT)]
        [ServiceLogIntercept]
        Task<ApiResult<List<SysDictionaryModel>>> GetSysDictionaryByCondition(SysDictionaryQuery query);
 

        [Authorization(AuthType = AuthorizationType.JWT)]
        [ServiceLogIntercept]
        [ServiceCacheIntercept(CachingMethod.Remove, "GetSysDictionaryByParentCode_{0}", CacheSectionType = "ddlCache", Mode = CacheTargetType.MemoryCache)]
        Task<ApiResult<bool>> DeleteById(string parentCode,List<int> ids);

        [Authorization(AuthType = AuthorizationType.JWT)]
        [ServiceLogIntercept]
        Task<ApiResult<bool>> Stop(List<int> ids);

        [Authorization(AuthType = AuthorizationType.JWT)]
        [ServiceLogIntercept]
        Task<ApiResult<bool>> Open(List<int> ids);

        [Authorization(AuthType = AuthorizationType.JWT)]
        [ServiceLogIntercept]
        [ServiceCacheIntercept(CachingMethod.Remove, "GetSysDictionaryByParentCode_{0}", CacheSectionType = "ddlCache", Mode = CacheTargetType.MemoryCache)]
        Task<ApiResult<bool>> Modify(SysDictionaryModel model);


        [Authorization(AuthType = AuthorizationType.JWT)]
        [ServiceLogIntercept]
        Task<ApiResult<bool>> Validate(SysDictionaryModel model);
    }
}

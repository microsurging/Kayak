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
    public interface ISysOrganizationService : IServiceKey
    {
        [Authorization(AuthType = AuthorizationType.JWT)]
        [ServiceCacheIntercept(CachingMethod.Remove, "GetOrgCompany", CacheSectionType = "ddlCache", Mode = CacheTargetType.MemoryCache, EnableStageCache = true)]
        Task<ApiResult<bool>> Add(SysOrganizationModel model);

        [Authorization(AuthType = AuthorizationType.JWT)]
        Task<ApiResult<SysOrganizationModel>> GetSysOrganization(int id);
        [Authorization(AuthType = AuthorizationType.JWT)]
        Task<ApiResult<Page<SysOrganizationModel>>> GetPageAsync(SysOrganizationQuery query); 
        [Authorization(AuthType = AuthorizationType.JWT)]
        Task<ApiResult<List<SysOrganizationModel>>> GetSysOrganizationByCondition(SysOrganizationQuery query);

        [Authorization(AuthType = AuthorizationType.JWT)]
        [ServiceCacheIntercept(CachingMethod.Remove, "GetOrgCompany", CacheSectionType = "ddlCache", Mode = CacheTargetType.MemoryCache, EnableStageCache = true)]
        Task<ApiResult<bool>> DeleteById(List<int> ids);

        [Authorization(AuthType = AuthorizationType.JWT)]
        [ServiceCacheIntercept(CachingMethod.Remove, "GetOrgCompany", CacheSectionType = "ddlCache", Mode = CacheTargetType.MemoryCache, EnableStageCache = true)]
        Task<ApiResult<bool>> Modify(SysOrganizationModel model);


        [Authorization(AuthType = AuthorizationType.JWT)]
        Task<ApiResult<bool>> Validate(SysOrganizationModel model);

        [Authorization(AuthType = AuthorizationType.JWT)]
        [ServiceCacheIntercept(CachingMethod.Get, Key = "GetOrgCompany", CacheSectionType = "ddlCache", EnableL2Cache = false, Mode = CacheTargetType.MemoryCache, Time = 480, EnableStageCache = true)]
        public Task<ApiResult<List<SysOrganizationModel>>> GetOrgCompany();
    }
}

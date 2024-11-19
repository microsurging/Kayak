using Kayak.Core.Common.Response;
using Kayak.IModuleServices.DeviceManage.Models;
using Kayak.IModuleServices.DeviceManage.Queries;
using Surging.Core.CPlatform.Filters.Implementation;
using Surging.Core.CPlatform.Ioc;
using Surging.Core.CPlatform.Runtime.Server.Implementation.ServiceDiscovery.Attributes;
using Surging.Core.ProxyGenerator.Interceptors.Implementation.Metadatas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kayak.IModuleServices.DeviceManage
{
    [ServiceBundle("api/{Service}/{Method}")]
    public interface IPropertyThresholdService : IServiceKey
    {
        [Authorization(AuthType = AuthorizationType.JWT)]
        [ServiceCacheIntercept(CachingMethod.Remove, "GetByPropertyId_{0}", CacheSectionType = "ddlCache", Mode = CacheTargetType.MemoryCache, EnableStageCache = true)]
        Task<ApiResult<bool>> Edit(string key, List<PropertyThresholdModel> list);

        [Authorization(AuthType = AuthorizationType.JWT)]
        Task<ApiResult<List<PropertyThresholdModel>>> GetByPropertyId(int propertyId);

        [Authorization(AuthType = AuthorizationType.JWT)]
        [ServiceCacheIntercept(CachingMethod.Get, Key = "GetByPropertyId_{0}_{1}_{2}", CacheSectionType = "ddlCache", EnableL2Cache = false, Mode = CacheTargetType.MemoryCache, Time = 480, EnableStageCache = true)]
        Task<ApiResult<List<PropertyThresholdModel>>> Get(PropertyThresholdQuery query);
    }
}

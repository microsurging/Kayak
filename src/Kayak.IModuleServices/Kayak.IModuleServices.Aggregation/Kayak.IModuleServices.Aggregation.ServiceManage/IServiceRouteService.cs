using Kayak.Core.Common.Repsitories.Implementation;
using Kayak.Core.Common.Response;
using Kayak.IModuleServices.Aggregation.ServiceManage.Models;
using Kayak.IModuleServices.Aggregation.ServiceManage.Queries;
using Kayak.IModuleServices.ServiceManage.Query;
using Surging.Core.CPlatform;
using Surging.Core.CPlatform.Address;
using Surging.Core.CPlatform.Filters.Implementation;
using Surging.Core.CPlatform.Ioc;
using Surging.Core.CPlatform.Routing;
using Surging.Core.CPlatform.Runtime.Server.Implementation.ServiceDiscovery.Attributes;
using Surging.Core.CPlatform.Support;
using Surging.Core.ProxyGenerator.Interceptors.Implementation.Metadatas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kayak.IModuleServices.Aggregation.ServiceManage
{
    [ServiceBundle("api/{Service}/{Method}", isPrefix: true)]
    public interface IServiceRouteService:IServiceKey
    {
        [Authorization(AuthType = AuthorizationType.JWT)]
        [ServiceRoute("query")]
        Task<ApiResult<Page<ServiceRoute>>> GetPageAsync(ServiceRouteQuery query);

        [Authorization(AuthType = AuthorizationType.JWT)]
        [ServiceCacheIntercept(CachingMethod.Get, Key = "GetStatistics_{0}", CacheSectionType = "ddlCache", EnableL2Cache = false, Mode = CacheTargetType.MemoryCache, Time = 480, EnableStageCache = true)]
        Task<ApiResult<ServiceStatisticsModel>> GetStatistics(int registryCenterType);

        [Authorization(AuthType = AuthorizationType.JWT)]
        Task<ApiResult<ServiceDescriptor>> GetServiceDescriptor(int registryCenterType,string serviceId);

        [Authorization(AuthType = AuthorizationType.JWT)]
        Task<ApiResult<Page<CheckIpAddressModel>>> GetAddresses(ServiceRouteQuery query);

        [Authorization(AuthType = AuthorizationType.JWT)]
        Task<ApiResult<ServiceCommand>> GetServiceCommand(int registryCenterType, string serviceId);

        [Authorization(AuthType = AuthorizationType.JWT)]
        Task<ApiResult<bool>> ModifyAddress(ModifyAddressParams model);

        [Authorization(AuthType = AuthorizationType.JWT)]
        Task<ApiResult<bool>> ModifyServiceDescriptor(EditServiceDescriptor model);

        [Authorization(AuthType = AuthorizationType.JWT)]
        Task<ApiResult<bool>> AddServiceDescriptor(EditServiceDescriptor model);

    }
}

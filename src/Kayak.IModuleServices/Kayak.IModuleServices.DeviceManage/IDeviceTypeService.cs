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
    public interface IDeviceTypeService : IServiceKey
    {
        [Authorization(AuthType = AuthorizationType.JWT)]
        [ServiceCacheIntercept(CachingMethod.Remove, "GetProducts", CacheSectionType = "ddlCache", Mode = CacheTargetType.MemoryCache, EnableStageCache = true)]
        Task<ApiResult<bool>> Add(DeviceTypeModel model);

        [Authorization(AuthType = AuthorizationType.JWT)]
        Task<ApiResult<DeviceTypeModel>> GetDeviceType(int id);

        [Authorization(AuthType = AuthorizationType.JWT)]
        Task<ApiResult<Page<DeviceTypeModel>>> GetPageAsync(DeviceTypeQuery query);

        [Authorization(AuthType = AuthorizationType.JWT)]
        Task<ApiResult<List<DeviceTypeModel>>> GetDeviceTypeByCondition(DeviceTypeQuery query);

        [Authorization(AuthType = AuthorizationType.JWT)]
        [ServiceCacheIntercept(CachingMethod.Remove, "GetDeviceTypes", CacheSectionType = "ddlCache", Mode = CacheTargetType.MemoryCache, EnableStageCache = true)]
        [ServiceLogIntercept]
        Task<ApiResult<bool>> DeleteById(List<int> ids);

        [Authorization(AuthType = AuthorizationType.JWT)]
        [ServiceCacheIntercept(CachingMethod.Remove, "GetDeviceTypes", CacheSectionType = "ddlCache", Mode = CacheTargetType.MemoryCache, EnableStageCache = true)]
        Task<ApiResult<bool>> Modify(DeviceTypeModel model);


        [Authorization(AuthType = AuthorizationType.JWT)]
        Task<ApiResult<bool>> Validate(DeviceTypeModel model);

        [Authorization(AuthType = AuthorizationType.JWT)]
        [ServiceCacheIntercept(CachingMethod.Get, Key = "GetDeviceTypes", CacheSectionType = "ddlCache", EnableL2Cache = false, Mode = CacheTargetType.MemoryCache, Time = 480, EnableStageCache = true)]
        Task<ApiResult<List<DeviceTypeModel>>> GetDeviceTypes();
    }
}

using Kayak.Core.Common.Response;
using Kayak.IModuleServices.DeviceManage.Models;
using Kayak.IModuleServices.DeviceManage.Queries;
using Surging.Core.CPlatform.Filters.Implementation;
using Surging.Core.CPlatform.Ioc;
using Surging.Core.CPlatform.Runtime.Server.Implementation.ServiceDiscovery.Attributes;
using Surging.Core.ProxyGenerator.Interceptors.Implementation.Metadatas;
using Surging.Core.System.Intercept;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CachingMethod = Surging.Core.ProxyGenerator.Interceptors.Implementation.Metadatas.CachingMethod;

namespace Kayak.IModuleServices.DeviceManage
{
    [ServiceBundle("api/{Service}/{Method}")]
    public interface IProductService : IServiceKey
    {
        [Authorization(AuthType = AuthorizationType.JWT)]
        [ServiceCacheIntercept(CachingMethod.Remove, "GetProducts", CacheSectionType = "ddlCache", Mode = CacheTargetType.MemoryCache, EnableStageCache = true)]
        Task<ApiResult<bool>> Add(ProductModel model);

        [Authorization(AuthType = AuthorizationType.JWT)]
        Task<ApiResult<ProductModel>> GetProduct(int id);

        [Authorization(AuthType = AuthorizationType.JWT)]
        Task<ApiResult<Page<ProductModel>>> GetPageAsync(ProductQuery query);

        [Authorization(AuthType = AuthorizationType.JWT)]
        Task<ApiResult<List<ProductModel>>> GetProductByCondition(ProductQuery query);

        [Authorization(AuthType = AuthorizationType.JWT)]
        [ServiceCacheIntercept(CachingMethod.Remove, "GetProducts", CacheSectionType = "ddlCache", Mode = CacheTargetType.MemoryCache, EnableStageCache = true)]
        [ServiceLogIntercept]
        Task<ApiResult<bool>> DeleteById(List<int> ids);

        [Authorization(AuthType = AuthorizationType.JWT)]
        [ServiceCacheIntercept(CachingMethod.Remove, "GetProducts", CacheSectionType = "ddlCache", Mode = CacheTargetType.MemoryCache, EnableStageCache = true)]
        Task<ApiResult<bool>> Modify(ProductModel model);


        [Authorization(AuthType = AuthorizationType.JWT)]
        Task<ApiResult<bool>> Validate(ProductModel model);

        [Authorization(AuthType = AuthorizationType.JWT)]
        [ServiceCacheIntercept(CachingMethod.Remove, "GetProducts",  CacheSectionType = "ddlCache", Mode = CacheTargetType.MemoryCache, EnableStageCache = true)]
        Task<ApiResult<bool>> Stop(List<int> ids);

        [Authorization(AuthType = AuthorizationType.JWT)]
        [ServiceCacheIntercept(CachingMethod.Remove, "GetProducts", CacheSectionType = "ddlCache", Mode = CacheTargetType.MemoryCache, EnableStageCache = true)]
        Task<ApiResult<bool>> Open(List<int> ids);

        [Authorization(AuthType = AuthorizationType.JWT)]
        [ServiceCacheIntercept(CachingMethod.Get, Key = "GetProducts", CacheSectionType = "ddlCache", EnableL2Cache = false, Mode = CacheTargetType.MemoryCache, Time = 480, EnableStageCache = true)]
        Task<ApiResult<List<ProductModel>>> GetProducts();

        [Authorization(AuthType = AuthorizationType.JWT)]
        [ServiceCacheIntercept(CachingMethod.Get, Key = "GetProductStatistics", CacheSectionType = "ddlCache", EnableL2Cache = false, Mode = CacheTargetType.MemoryCache, Time = 1, EnableStageCache = true)]
        Task<ApiResult<ProductStatisticsModel>> GetProductStatistics();


    }
}

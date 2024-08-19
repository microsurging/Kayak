using Kayak.Core.Common.Response;
using Kayak.IModuleServices.Aggregation.ServiceManage.Models;
using Kayak.IModuleServices.ServiceManage.Query;
using Surging.Core.CPlatform.Filters.Implementation;
using Surging.Core.CPlatform.Ioc;
using Surging.Core.CPlatform.Runtime.Server.Implementation.ServiceDiscovery.Attributes;

namespace Kayak.IModuleServices.Aggregation.ServiceManage
{
    [ServiceBundle("api/{Service}/{Method}", isPrefix: true)]
    public interface IRegCenterAggregationService : IServiceKey
    {
        [Authorization(AuthType = AuthorizationType.JWT)]
        [ServiceRoute("query")]
        Task<ApiResult<Page<RegCenterAggregationModel>>> GetPageAsync(RegistryCenterQuery query);
         
    }
}

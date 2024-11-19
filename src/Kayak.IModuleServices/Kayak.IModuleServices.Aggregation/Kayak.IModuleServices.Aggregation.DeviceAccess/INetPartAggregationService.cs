using Kayak.Core.Common.Response;
using Kayak.IModuleServices.Aggregation.DeviceAccess.Models;
using Kayak.IModuleServices.DeviceAccess.Models;
using Kayak.IModuleServices.DeviceAccess.Queries;
using Surging.Core.CPlatform.Filters.Implementation;
using Surging.Core.CPlatform.Ioc;
using Surging.Core.CPlatform.Runtime.Server.Implementation.ServiceDiscovery.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kayak.IModuleServices.Aggregation.DeviceAccess
{
    [ServiceBundle("api/{Service}/{Method}")]
    public interface INetPartAggregationService:IServiceKey
    {
        [Authorization(AuthType = AuthorizationType.JWT)]
        Task<ApiResult<Page<NetPartAggregationModel>>> GetPageAsync(NetworkPartQuery query);

        [Authorization(AuthType = AuthorizationType.JWT)]
        Task<ApiResult<bool>> CreateNetwork(int id);

        [Authorization(AuthType = AuthorizationType.JWT)]
        Task<ApiResult<bool>> ShundownNetwork(int id);

        [Authorization(AuthType = AuthorizationType.JWT)]
        Task<ApiResult<bool>> Modify(NetworkPartModel model);

        [Authorization(AuthType = AuthorizationType.JWT)]
        Task<ApiResult<Page<NetworkPartModel>>> GetPageListAsync(NetworkPartQuery query);
    }
}

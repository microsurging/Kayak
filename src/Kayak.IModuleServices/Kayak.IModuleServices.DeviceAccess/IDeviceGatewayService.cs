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
    public interface IDeviceGatewayService : IServiceKey
    {
        [Authorization(AuthType = AuthorizationType.JWT)]
        Task<ApiResult<bool>> Add(DeviceGatewayModel model);

        [Authorization(AuthType = AuthorizationType.JWT)]
        Task<ApiResult<bool>> Modify(DeviceGatewayModel model);
         
        [Authorization(AuthType = AuthorizationType.JWT)]
        Task<ApiResult<Page<DeviceGatewayModel>>> GetPageAsync(DeviceGatewayQuery query);

        [Authorization(AuthType = AuthorizationType.JWT)]
        Task<ApiResult<DeviceGatewayModel>> GetModelById(int gatewayId);

        [Authorization(AuthType = AuthorizationType.JWT)]
        Task<ApiResult<bool>> Stop(List<int> ids);

        [Authorization(AuthType = AuthorizationType.JWT)]
        Task<ApiResult<bool>> Open(List<int> ids);

        [Authorization(AuthType = AuthorizationType.JWT)]
        Task<ApiResult<bool>> DeleteById(List<int> ids);
    }
}

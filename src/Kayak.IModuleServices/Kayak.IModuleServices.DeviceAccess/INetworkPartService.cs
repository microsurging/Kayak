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
    public interface INetworkPartService : IServiceKey
    {
        [Authorization(AuthType = AuthorizationType.JWT)]
        Task<ApiResult<bool>> Add(NetworkPartModel model);
         
        [Authorization(AuthType = AuthorizationType.JWT)]
        Task<ApiResult<List<NetworkPartModel>>> GetListByIds(List<int> ids);

        [Authorization(AuthType = AuthorizationType.JWT)]
        Task<ApiResult<Page<NetworkPartModel>>> GetPageAsync(NetworkPartQuery query);
       
        [Authorization(AuthType = AuthorizationType.JWT)]
        Task<ApiResult<NetworkPartModel>> GetNetworkPartById(int id);

        [Authorization(AuthType = AuthorizationType.JWT)]
        Task<ApiResult<List<NetworkPartModel>>> GetNetworkPartByCondition(NetworkPartQuery query);

        [Authorization(AuthType = AuthorizationType.JWT)]
        Task<ApiResult<bool>> DeleteById(List<int> ids);

        [Authorization(AuthType = AuthorizationType.JWT)]
        Task<ApiResult<bool>> Modify(NetworkPartModel model);


        [Authorization(AuthType = AuthorizationType.JWT)]
        Task<ApiResult<bool>> Validate(NetworkPartModel model);

        [Authorization(AuthType = AuthorizationType.JWT)]
        Task<ApiResult<bool>> Stop(List<int> ids);

        [Authorization(AuthType = AuthorizationType.JWT)]
        Task<ApiResult<bool>> Open(List<int> ids);
    }
}

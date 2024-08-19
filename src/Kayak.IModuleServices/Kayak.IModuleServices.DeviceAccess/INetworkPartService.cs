using Kayak.Core.Common.Response;
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

namespace Kayak.IModuleServices.DeviceAccess
{
    [ServiceBundle("api/{Service}/{Method}")]
    public interface INetworkPartService : IServiceKey
    {
        [Authorization(AuthType = AuthorizationType.JWT)]
        Task<ApiResult<bool>> Add(NetworkPartModel model);

        [Authorization(AuthType = AuthorizationType.JWT)]
        Task<ApiResult<NetworkPartModel>> GetProtocol(int id);

        [Authorization(AuthType = AuthorizationType.JWT)]
        Task<ApiResult<Page<NetworkPartModel>>> GetPageAsync(NetworkPartQuery query);
    }
}

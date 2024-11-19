using Kayak.Core.Common.Response;
using Kayak.IModuleServices.Aggregation.DeviceAccess.Models;
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

namespace Kayak.IModuleServices.Aggregation.DeviceAccess
{
    [ServiceBundle("api/{Service}/{Method}")]
    public interface IProtocolManageAggService : IServiceKey
    {
        [Authorization(AuthType = AuthorizationType.JWT)]
        Task<ApiResult<bool>> Add(ProtocolAggModel model);

        [Authorization(AuthType = AuthorizationType.JWT)] 
        Task<ApiResult<bool>> Modify(ProtocolAggModel model);

        [Authorization(AuthType = AuthorizationType.JWT)]
        Task<ApiResult<bool>> Republish(List<int> ids);

        [Authorization(AuthType = AuthorizationType.JWT)]
        Task<ApiResult<bool>> CancelPublish(List<int> ids);


        [Authorization(AuthType = AuthorizationType.JWT)]
        Task<ApiResult<Page<ProtocolModel>>> GetPageListAsync(ProtocolQuery query);

    }
}

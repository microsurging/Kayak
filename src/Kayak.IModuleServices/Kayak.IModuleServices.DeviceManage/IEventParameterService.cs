using Kayak.Core.Common.Response;
using Kayak.IModuleServices.DeviceManage.Models;
using Kayak.IModuleServices.DeviceManage.Queries;
using Surging.Core.CPlatform.Filters.Implementation;
using Surging.Core.CPlatform.Ioc;
using Surging.Core.CPlatform.Runtime.Server.Implementation.ServiceDiscovery.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kayak.IModuleServices.DeviceManage
{
    [ServiceBundle("api/{Service}/{Method}")]
    public interface IEventParameterService : IServiceKey
    {
        [Authorization(AuthType = AuthorizationType.JWT)]
        Task<ApiResult<bool>> Edit(string key, List<EventParameterModel> list);

        [Authorization(AuthType = AuthorizationType.JWT)]
        Task<ApiResult<List<EventParameterModel>>> GetByEventId(int eventId);

        [Authorization(AuthType = AuthorizationType.JWT)]
        Task<ApiResult<List<EventParameterModel>>> Get(EventParameterQuery query);
    }
}

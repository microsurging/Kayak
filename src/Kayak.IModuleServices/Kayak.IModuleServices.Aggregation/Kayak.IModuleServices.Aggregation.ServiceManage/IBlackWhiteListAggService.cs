using Kayak.Core.Common.Response;
using Kayak.IModuleServices.Aggregation.ServiceManage.Models;
using Kayak.IModuleServices.Aggregation.ServiceManage.Queries;
using Kayak.IModuleServices.ServiceManage.Model;
using Kayak.IModuleServices.ServiceManage.Query;
using Surging.Core.CPlatform.Filters.Implementation;
using Surging.Core.CPlatform.Ioc;
using Surging.Core.CPlatform.Runtime.Server.Implementation.ServiceDiscovery.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kayak.IModuleServices.Aggregation.ServiceManage
{
    [ServiceBundle("api/{Service}/{Method}")]
    public interface IBlackWhiteListAggService : IServiceKey
    {
        [Authorization(AuthType = AuthorizationType.JWT)]
        Task<ApiResult<Page<BlackWhiteListModel>>> GetPageAsync(BlackWhiteListQuery query);

        [Authorization(AuthType = AuthorizationType.JWT)]
        Task<ApiResult<bool>> Enable(List<int> ids);

        [Authorization(AuthType = AuthorizationType.JWT)]
        Task<ApiResult<bool>> Disable(List<int> ids);
    }
}
 
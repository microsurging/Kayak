using Kayak.Core.Common.Intercepte.Model;
using Kayak.Core.Common.Intercepte.Queries;
using Kayak.Core.Common.Response;
using Surging.Core.CPlatform.Filters.Implementation;
using Surging.Core.CPlatform.Ioc;
using Surging.Core.CPlatform.Runtime.Server.Implementation.ServiceDiscovery.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kayak.Core.Common.Intercepte
{
    [ServiceBundle("api/{Service}/{Method}")]
    public interface IOperateLogService : IServiceKey
    {
        [Authorization(AuthType = AuthorizationType.JWT)]
        Task<ApiResult<Page<OperateLogModel>>> GetPageAsync(OperateLogQuery query);

        [Authorization(AuthType = AuthorizationType.JWT)]
        Task<ApiResult<bool>> DeleteById(List<int> ids);

        [Authorization(AuthType = AuthorizationType.JWT)]
        Task<ApiResult<bool>> Add(OperateLogModel model);
    }
}

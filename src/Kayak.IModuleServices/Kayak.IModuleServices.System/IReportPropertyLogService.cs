using Kayak.Core.Common.Response;
using Kayak.IModuleServices.System.Models;
using Kayak.IModuleServices.System.Queries;
using Surging.Core.CPlatform.Filters.Implementation;
using Surging.Core.CPlatform.Ioc;
using Surging.Core.CPlatform.Runtime.Server.Implementation.ServiceDiscovery.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kayak.IModuleServices.System
{
    [ServiceBundle("api/{Service}/{Method}")]
    public interface IReportPropertyLogService : IServiceKey
    {
        [Authorization(AuthType = AuthorizationType.JWT)]
        Task<ApiResult<Page<ReportPropertyLogModel>>> GetPageAsync(ReportPropertyLogQuery query);

        [Authorization(AuthType = AuthorizationType.JWT)]
        Task<ApiResult<ReportPropertyLogModel>> GetById(int id);

        [Authorization(AuthType = AuthorizationType.JWT)]
        Task<ApiResult<bool>> Add(ReportPropertyLogModel model);

        [Authorization(AuthType = AuthorizationType.JWT)]
        Task<ApiResult<bool>> DeleteById(List<int> ids);
    }
}

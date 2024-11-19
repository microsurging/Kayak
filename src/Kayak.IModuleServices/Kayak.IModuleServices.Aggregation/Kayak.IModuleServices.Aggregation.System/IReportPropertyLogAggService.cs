using Kayak.Core.Common.Response;
using Kayak.IModuleServices.Aggregation.System.Models;
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

namespace Kayak.IModuleServices.Aggregation.System
{
    [ServiceBundle("api/{Service}/{Method}")]
    public interface IReportPropertyLogAggService : IServiceKey
    {
        [Authorization(AuthType = AuthorizationType.JWT)]
        Task<ApiResult<Page<ReportPropertyLogAggModel>>> GetPageAsync(Globalization? global, ReportPropertyLogQuery query);
    }
}

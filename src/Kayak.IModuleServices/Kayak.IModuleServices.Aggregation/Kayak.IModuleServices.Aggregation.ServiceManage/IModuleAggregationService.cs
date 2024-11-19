using Kayak.Core.Common.Response;
using Kayak.IModuleServices.Aggregation.ServiceManage.Models;
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
    public interface IModuleAggregationService : IServiceKey
    {
        [Authorization(AuthType = AuthorizationType.JWT)]
        Task<ApiResult<bool>> Add(ModuleAggModel model);

        [Authorization(AuthType = AuthorizationType.JWT)]
        Task<ApiResult<bool>> Modify(ModuleAggModel model);
    }
}

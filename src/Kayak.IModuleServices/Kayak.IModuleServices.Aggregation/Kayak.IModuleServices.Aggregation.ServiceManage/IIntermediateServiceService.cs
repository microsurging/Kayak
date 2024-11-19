using Kayak.Core.Common.Response;
using Kayak.IModuleServices.Aggregation.ServiceManage.Models;
using Kayak.IModuleServices.Aggregation.ServiceManage.Queries;
using Surging.Core.CPlatform;
using Surging.Core.CPlatform.Filters.Implementation;
using Surging.Core.CPlatform.Ioc;
using Surging.Core.CPlatform.Routing;
using Surging.Core.CPlatform.Runtime.Server;
using Surging.Core.CPlatform.Runtime.Server.Implementation.ServiceDiscovery.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kayak.IModuleServices.Aggregation.ServiceManage
{
    [ServiceBundle("api/{Service}/{Method}", isPrefix: true)]
    public interface IIntermediateServiceService : IServiceKey
    {

        [Authorization(AuthType = AuthorizationType.JWT)]
        [ServiceRoute("query")]
        Task<ApiResult<Page<ServiceEntryModel>>> GetPageAsync(IntermediateServiceQuery query);

        [Authorization(AuthType = AuthorizationType.JWT)]
        Task<ApiResult<ServiceEntryModel>> GetByServiceId(string serviceId);

        [Authorization(AuthType = AuthorizationType.JWT)]
        Task<ApiResult<ServiceDescriptor>> GetServiceDescriptor(string serviceId);
    }
}

using Kayak.Core.Common.Response;
using Kayak.IModuleServices.Aggregation.DeviceManage.Models;
using Kayak.IModuleServices.DeviceManage.Models;
using Surging.Core.CPlatform;
using Surging.Core.CPlatform.Filters.Implementation;
using Surging.Core.CPlatform.Ioc;
using Surging.Core.CPlatform.Runtime.Server.Implementation.ServiceDiscovery.Attributes;
using Surging.Core.DeviceGateway.Runtime.Core.Metadata;
using Surging.Core.KestrelHttpServer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kayak.IModuleServices.Aggregation.DeviceManage
{
    [ServiceBundle("api/{Service}/{Method}")]
    public interface IDeviceAccessAggService : IServiceKey
    {
        [Authorization(AuthType = AuthorizationType.JWT)]
        Task<ApiResult<ConfigMetadata>> GetPropertyConfig(string productCode);

        [Authorization(AuthType = AuthorizationType.JWT)]
        Task<IActionResult> DownDocumentFile(string productCode);

        [Authorization(AuthType = AuthorizationType.JWT)]
        Task<ApiResult<DeviceAccessAggModel>> GetByProductCode(string productCode);

        [Authorization(AuthType = AuthorizationType.JWT)]
        Task<ApiResult<List<ServiceDescriptor>>> GetRoutes(string productCode);
    }
}

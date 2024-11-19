using Kayak.Core.Common.Response;
using Kayak.IModuleServices.Aggregation.DeviceManage.Models;
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

namespace Kayak.IModuleServices.Aggregation.DeviceManage
{
    [ServiceBundle("api/{Service}/{Method}")]
    public interface IDeviceTypeAggService : IServiceKey
    {
        [Authorization(AuthType = AuthorizationType.JWT)]
        Task<ApiResult<Page<DeviceTypeAggregationModel>>> GetPageAsync(DeviceTypeQuery query);

        [Authorization(AuthType = AuthorizationType.JWT)]
        Task<ApiResult<List<DeviceTypeModel>>> GetDeviceTypes();

        [Authorization(AuthType = AuthorizationType.JWT)]
        Task<ApiResult<DeviceTypeAggregationModel>> GetDeviceTypeById(int id);
    }
}

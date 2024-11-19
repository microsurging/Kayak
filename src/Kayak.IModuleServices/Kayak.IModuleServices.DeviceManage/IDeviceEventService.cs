using Kayak.Core.Common.Response;
using Kayak.IModuleServices.DeviceManage.Models;
using Kayak.IModuleServices.DeviceManage.Queries;
using Surging.Core.CPlatform.Filters.Implementation;
using Surging.Core.CPlatform.Ioc;
using Surging.Core.CPlatform.Runtime.Server.Implementation.ServiceDiscovery.Attributes;
using Surging.Core.ProxyGenerator.Interceptors.Implementation.Metadatas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kayak.IModuleServices.DeviceManage
{
    [ServiceBundle("api/{Service}/{Method}")]
    public interface IDeviceEventService : IServiceKey
    {
        [Authorization(AuthType = AuthorizationType.JWT)]
        Task<ApiResult<Page<DeviceEventModel>>> GetPageAsync(DeviceEventQuery query);

        [Authorization(AuthType = AuthorizationType.JWT)]
        Task<ApiResult<bool>> DeleteById(List<int> ids);

        [Authorization(AuthType = AuthorizationType.JWT)]
        Task<ApiResult<bool>> Add(DeviceEventModel model);

        [Authorization(AuthType = AuthorizationType.JWT)]
        Task<ApiResult<List<DeviceEventModel>>> GetListByDeviceIds(string deviceId, List<string> eventIds);

        [Authorization(AuthType = AuthorizationType.JWT)]
        Task<ApiResult<int>> GetStatistics(DateTime startDate, DateTime endDate);


        [Authorization(AuthType = AuthorizationType.JWT)]
        Task<ApiResult<List<GroupStatistics>>> GetGroupStatistics(StatisticsQuery query);
    }
}


using DotNetty.Buffers;
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
    public interface IReportPropertyService:IServiceKey
    {
        [Authorization(AuthType = AuthorizationType.JWT)]
        Task<ApiResult<Page<ReportPropertyModel>>> GetPageAsync(ReportPropertyQuery query);

        [Authorization(AuthType = AuthorizationType.JWT)]
        Task<ApiResult<bool>> DeleteById(List<int> ids);

        [Authorization(AuthType = AuthorizationType.JWT)]
        Task<ApiResult<bool>> Add(ReportPropertyModel model);

        [Authorization(AuthType = AuthorizationType.JWT)]
        Task<ApiResult<List<ReportPropertyModel>>> GetListByDeviceIds(string deviceId, List<string> propertyIds);

        [Authorization(AuthType = AuthorizationType.JWT)]
        Task<ApiResult<List<GroupStatistics>>> GetGroupStatistics(StatisticsQuery query);

        [Authorization(AuthType = AuthorizationType.JWT)]
        Task<ApiResult<int>> GetStatistics(DateTime startDate, DateTime endDate);
    }
}

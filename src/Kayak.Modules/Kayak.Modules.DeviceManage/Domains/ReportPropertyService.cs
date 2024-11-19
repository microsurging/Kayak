using Kayak.Core.Common.Response;
using Kayak.IModuleServices.DeviceManage;
using Kayak.IModuleServices.DeviceManage.Models;
using Kayak.IModuleServices.DeviceManage.Queries;
using Kayak.Modules.DeviceManage.Repositories;
using Surging.Core.CPlatform.Ioc;
using Surging.Core.ProxyGenerator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Kayak.Modules.DeviceManage.Domains
{
    public class ReportPropertyService : ProxyServiceBase, IReportPropertyService, ISingleInstance
    {
        private readonly ReportPropertyRepository _repository;
        public ReportPropertyService(ReportPropertyRepository repository)
        {
            _repository = repository;
        }
        public async Task<ApiResult<bool>> Add(ReportPropertyModel model)
        {
            var result = await _repository.Add(model);
            return ApiResult<bool>.Succeed(result);
        }

        public async Task<ApiResult<bool>> DeleteById(List<int> ids)
        {
            var result = await _repository.DeleteById(ids);
            return ApiResult<bool>.Succeed(result);
        }

        public async Task<ApiResult<List<ReportPropertyModel>>> GetListByDeviceIds(string deviceId,List<string> propertyIds)
        {
            var result = await _repository.GetListByDeviceIds(deviceId, propertyIds);
            return ApiResult<List<ReportPropertyModel>>.Succeed(result);
        }

        public async Task<ApiResult<Page<ReportPropertyModel>>> GetPageAsync(ReportPropertyQuery query)
        {
            var result = await _repository.GetPageAsync(query);
            return ApiResult<Page<ReportPropertyModel>>.Succeed(result);
        }

        public async  Task<ApiResult<List<GroupStatistics>>> GetGroupStatistics(StatisticsQuery query)
        {
            var result = await _repository.GetGroupStatistics(query);
            return ApiResult<List<GroupStatistics>>.Succeed(result);
        }

        public async Task<ApiResult<int>> GetStatistics(DateTime startDate,DateTime endDate)
        {
            var result = await _repository.GetStatistics(startDate,endDate);
            return ApiResult<int>.Succeed(result);
        }

    }
}

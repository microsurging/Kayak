using Kayak.Core.Common.Intercepte;
using Kayak.Core.Common.Repsitories.Implementation;
using Kayak.Core.Common.Response;
using Kayak.IModuleServices.System;
using Kayak.IModuleServices.System.Models;
using Kayak.IModuleServices.System.Queries;
using Kayak.Modules.System.Repositories;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Surging.Core.CPlatform.Ioc;
using Surging.Core.ProxyGenerator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Kayak.Modules.System.Domians
{
    public class ReportPropertyLogService : ProxyServiceBase, ISingleInstance, IReportPropertyLogService
    {
        private readonly ReportPropertyLogRepository _repository;
        public ReportPropertyLogService(ReportPropertyLogRepository repository)
        {
            _repository = repository;
        }

        public async Task<ApiResult<ReportPropertyLogModel>> GetById(int id)
        {
            var result = await _repository.GetById(id);
            return ApiResult<ReportPropertyLogModel>.Succeed(result);
        }

        public async Task<ApiResult<bool>> Add(ReportPropertyLogModel model)
        {
            var result = await _repository.Add(model);
            return ApiResult<bool>.Succeed(result);
        }

        public async Task<ApiResult<bool>> DeleteById(List<int> ids)
        {
            var result = await _repository.DelBatch(ids);
            return ApiResult<bool>.Succeed(result);
        }

        public async Task<ApiResult<Page<ReportPropertyLogModel>>> GetPageAsync(ReportPropertyLogQuery query)
        {
            var result = await _repository.GetPageAsync(query);
            return ApiResult<Page<ReportPropertyLogModel>>.Succeed(result);
        }
    }
}

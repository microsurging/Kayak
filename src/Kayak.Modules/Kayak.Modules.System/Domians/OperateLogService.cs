using Kayak.Core.Common.Intercepte;
using Kayak.Core.Common.Intercepte.Model;
using Kayak.Core.Common.Intercepte.Queries;
using Kayak.Core.Common.Response;
using Kayak.IModuleServices.System;
using Kayak.IModuleServices.System.Models;
using Kayak.Modules.System.Repositories;
using Surging.Core.CPlatform.Ioc;
using Surging.Core.ProxyGenerator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kayak.Modules.System.Domians
{
    public class OperateLogService : ProxyServiceBase, ISingleInstance, IOperateLogService
    {
        private readonly OperateLogRepository _repository;
        public OperateLogService(OperateLogRepository repository)
        { 
            _repository = repository;
        }

        public async Task<ApiResult<bool>> Add(OperateLogModel model)
        {
            var result = await _repository.Add(model);
            return ApiResult<bool>.Succeed(result);
        }

        public async Task<ApiResult<bool>> DeleteById(List<int> ids)
        {
            var result = await _repository.DeleteById(ids);
            return ApiResult<bool>.Succeed(result);
        }

        public async Task<ApiResult<Page<OperateLogModel>>> GetPageAsync(OperateLogQuery query)
        {
            var result = await _repository.GetPageAsync(query);
            return ApiResult<Page<OperateLogModel>>.Succeed(result);
        }
    }
}

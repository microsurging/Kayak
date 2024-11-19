using Kayak.Core.Common.Intercepte.Model;
using Kayak.Core.Common.Intercepte.Queries;
using Kayak.Core.Common.Intercepte;
using Kayak.Core.Common.Response;
using Kayak.Modules.System.Repositories;
using Surging.Core.CPlatform.Ioc;
using Surging.Core.ProxyGenerator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kayak.IModuleServices.System;
using Kayak.IModuleServices.System.Models;
using Kayak.IModuleServices.System.Queries;

namespace Kayak.Modules.System.Domians
{
    internal class NetworkLogService : ProxyServiceBase, ISingleInstance, INetworkLogService
    {
        private readonly NetworkLogRepository _repository;
        public NetworkLogService(NetworkLogRepository repository)
        {
            _repository = repository;
        }

        public async Task<ApiResult<bool>> Add(NetworkLogModel model)
        {
            var result = await _repository.Add(model);
            return ApiResult<bool>.Succeed(result);
        }

        public async Task<ApiResult<bool>> DeleteById(List<int> ids)
        {
            var result = await _repository.DeleteById(ids);
            return ApiResult<bool>.Succeed(result);
        }

        public async Task<ApiResult<Page<NetworkLogModel>>> GetPageAsync(NetworkLogQuery query)
        {
            var result = await _repository.GetPageAsync(query);
            return ApiResult<Page<NetworkLogModel>>.Succeed(result);
        }
    }
}
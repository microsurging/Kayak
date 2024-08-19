using Kayak.Core.Common.Response;
using Kayak.IModuleServices.DeviceAccess;
using Kayak.IModuleServices.DeviceAccess.Models;
using Kayak.IModuleServices.DeviceAccess.Queries;
using Kayak.Modules.DeviceAccess.Repositories;
using Surging.Core.CPlatform.Ioc;
using Surging.Core.ProxyGenerator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kayak.Modules.DeviceAccess.Domains
{
    public class NetworkPartService : ProxyServiceBase, ISingleInstance, INetworkPartService
    {
        private readonly NetworkPartRepository _repository;

        public NetworkPartService(NetworkPartRepository repository) {
            _repository = repository;
        }

        public async  Task<ApiResult<bool>> Add(NetworkPartModel model)
        {
            var result = await _repository.Add(model);
            return ApiResult<bool>.Succeed(result);
        }

        public async  Task<ApiResult<Page<NetworkPartModel>>> GetPageAsync(NetworkPartQuery query)
        {
            var result = await _repository.GetPageAsync(query);
            return ApiResult<Page<NetworkPartModel>>.Succeed(result);
        }

        public async  Task<ApiResult<NetworkPartModel>> GetProtocol(int id)
        {
            var result = await _repository.GetProtocol(id);
            return ApiResult<NetworkPartModel>.Succeed(result);
        }
    }
}

using Kayak.Core.Common.Enums;
using Kayak.Core.Common.Extensions;
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

        public async Task<ApiResult<List<NetworkPartModel>>> GetListByIds(List<int> ids)
        {
            var result = await _repository.GetListByIds(ids);
            return ApiResult<List<NetworkPartModel>>.Succeed(result);
        }
         
        public async Task<ApiResult<List<NetworkPartModel>>> GetNetworkPartByCondition(NetworkPartQuery query)
        {
            var result = await _repository.GetNetworkPartByCondition(query);
            return ApiResult<List<NetworkPartModel>>.Succeed(result);
        }

        public async Task<ApiResult<bool>> DeleteById(List<int> ids)
        {
            var result = await _repository.DeleteById(ids);
            return ApiResult<bool>.Succeed(result);
        }

        public async Task<ApiResult<bool>> Modify(NetworkPartModel model)
        {
            var result = await _repository.Modify(model);
            return ApiResult<bool>.Succeed(result);
        }

        public async Task<ApiResult<bool>> Validate(NetworkPartModel model)
        {
            var message = "";
            if (!model.Name.IsNullOrEmpty())
            {
                var result = await _repository.ExistsModelByName(model.Name);
                message = result ? "组件名称已存在" : message;
            }

            return message.IsNullOrEmpty() ? ApiResult<bool>.Succeed(true) : ApiResult<bool>.Failure(EnumReturnCode.CUSTOM_ERROR, message);
        }

        public async Task<ApiResult<bool>> Stop(List<int> ids)
        {
            var result = await _repository.Stop(ids);
            return ApiResult<bool>.Succeed(result);
        }

        public async Task<ApiResult<bool>> Open(List<int> ids)
        {
            var result = await _repository.Open(ids);
            return ApiResult<bool>.Succeed(result);
        }

        public  async Task<ApiResult<NetworkPartModel>> GetNetworkPartById(int id)
        {
            var result = await _repository.GetNetworkPartById(id);
            return ApiResult<NetworkPartModel>.Succeed(result);
        }
    }
}

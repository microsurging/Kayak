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
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Kayak.Modules.DeviceAccess.Domains
{
    public class DeviceGatewayService : ProxyServiceBase, ISingleInstance, IDeviceGatewayService
    {
        private readonly DeviceGatewayRepository _repository;

        public DeviceGatewayService(DeviceGatewayRepository repository)
        {
            _repository = repository;
        }
        public async Task<ApiResult<bool>> Add(DeviceGatewayModel model)
        {
            var result = await _repository.Add(model);
            return ApiResult<bool>.Succeed(result);
        }

        public async Task<ApiResult<bool>> DeleteById(List<int> ids)
        {
            var result = await _repository.DeleteById(ids);
            return ApiResult<bool>.Succeed(result);
        }

        public async Task<ApiResult<DeviceGatewayModel>> GetModelById(int gatewayId)
        {
            var result = await _repository.GetModelById(gatewayId);
            return ApiResult<DeviceGatewayModel>.Succeed(result);
        }

        public async Task<ApiResult<Page<DeviceGatewayModel>>> GetPageAsync(DeviceGatewayQuery query)
        {
            var result = await _repository.GetPageAsync(query);
            return ApiResult<Page<DeviceGatewayModel>>.Succeed(result);
        }
         
        public async Task<ApiResult<bool>> Modify(DeviceGatewayModel model)
        {
            var result = await _repository.Modify(model);
            return ApiResult<bool>.Succeed(result);
        }

        public async Task<ApiResult<bool>> Open(List<int> ids)
        {
            var result = await _repository.Open(ids);
            return ApiResult<bool>.Succeed(result);
        }

        public async Task<ApiResult<bool>> Stop(List<int> ids)
        {
            var result = await _repository.Stop(ids);
            return ApiResult<bool>.Succeed(result);
        }
    }
}

using Kayak.Core.Common.Response;
using Kayak.IModuleServices.DeviceManage;
using Kayak.IModuleServices.DeviceManage.Models;
using Kayak.Modules.DeviceManage.Repositories;
using Surging.Core.CPlatform.Ioc;
using Surging.Core.ProxyGenerator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kayak.Modules.DeviceManage.Domains
{
    public class DeviceConfigService : ProxyServiceBase, IDeviceConfigService, ISingleInstance
    {
        private readonly DeviceConfigRepository _repository;
        public DeviceConfigService(DeviceConfigRepository repository)
        {
            _repository = repository;
        }
        public async Task<ApiResult<bool>> Add(DeviceConfigModel model)
        {
            var result = await _repository.Add(model);
            return ApiResult<bool>.Succeed(result);
        }

        public async Task<ApiResult<DeviceConfigModel>> GetByDeviceCode(string deviceCode)
        {
            var result = await _repository.GetByDeviceCode(deviceCode);
            return ApiResult<DeviceConfigModel>.Succeed(result);
        }

        public async Task<ApiResult<bool>> Modify(DeviceConfigModel model)
        {
            var result = await _repository.Modify(model);
            return ApiResult<bool>.Succeed(result);
        }
    }
}

using Kayak.Core.Common.Repsitories.Implementation;
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
    internal class DeviceAccessService : ProxyServiceBase, IDeviceAccessService, ISingleInstance
    {
        private readonly DeviceAccessRepository _repository;
        public DeviceAccessService(DeviceAccessRepository repository) {
            _repository = repository;
        }
        public async Task<ApiResult<bool>> Add(DeviceAccessModel model)
        {
            var result = await _repository.Add(model);
            return ApiResult<bool>.Succeed(result);
        }

        public async Task<ApiResult<DeviceAccessModel>> GetByProductCode(string productCode)
        {
            var result = await _repository.GetByProductCode(productCode);
            return ApiResult<DeviceAccessModel>.Succeed(result);
        }

        public async Task<ApiResult<bool>> Modify(DeviceAccessModel model)
        {
            var result = await _repository.Modify(model);
            return ApiResult<bool>.Succeed(result);
        }
    }
}

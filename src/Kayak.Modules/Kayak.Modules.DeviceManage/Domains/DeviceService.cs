using Kayak.Core.Common.Enums;
using Kayak.Core.Common.Extensions;
using Kayak.Core.Common.Repsitories.Implementation;
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
    public class DeviceService : ProxyServiceBase, IDeviceService, ISingleInstance
    {
        private readonly DeviceRepository _repository;
        public DeviceService(DeviceRepository repository)
        {
            _repository = repository;
        }
        public async Task<ApiResult<bool>> Add(DeviceModel model)
        {
            var result = await _repository.Add(model);
            return ApiResult<bool>.Succeed(result);
        }

        public async Task<ApiResult<bool>> DeleteById(List<int> ids)
        {
            var result = await _repository.DeleteById(ids);
            return ApiResult<bool>.Succeed(result);
        }

        public async Task<ApiResult<List<DeviceModel>>> GetDeviceByCondition(DeviceQuery query)
        {
            var result = await _repository.GetDeviceByCondition(query);
            return ApiResult<List<DeviceModel>>.Succeed(result);
        }

        public async Task<ApiResult<Page<DeviceModel>>> GetPageAsync(DeviceQuery query)
        {
            var result = await _repository.GetPageAsync(query);
            return ApiResult<Page<DeviceModel>>.Succeed(result);
        }

        public async Task<ApiResult<List<DeviceModel>>> GetDeviceByIds(List<int> ids)
        {
            var result = await _repository.GetDeviceByIds(ids);
            return ApiResult<List<DeviceModel>>.Succeed(result);
        }

        public async Task<ApiResult<bool>> Modify(DeviceModel model)
        {
            var result = await _repository.Modify(model);
            return ApiResult<bool>.Succeed(result);
        }

        public async Task<ApiResult<bool>> Validate(DeviceModel model)
        {
            var message = "";
            if (!model.Code.IsNullOrEmpty())
            {
                var result = await _repository.ExistsModelByCode(model.Code);
                message = result ? "设备编码已存在" : message;
            }
            return message.IsNullOrEmpty() ? ApiResult<bool>.Succeed(true) : ApiResult<bool>.Failure(EnumReturnCode.CUSTOM_ERROR, message);
        }

        public async Task<ApiResult<bool>> ChangeDisable(List<int> ids)
        {
            var result = await _repository.ChangeDisable(ids);
            return ApiResult<bool>.Succeed(result);
        }

        public async Task<ApiResult<bool>> ChangeEnable(List<int> ids)
        {
            var result = await _repository.ChangeEnable(ids);
            return ApiResult<bool>.Succeed(result);
        }
    }
}

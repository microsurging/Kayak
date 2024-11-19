using Kayak.Core.Common.Enums;
using Kayak.Core.Common.Extensions;
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
    public class DeviceTypeService : ProxyServiceBase, IDeviceTypeService, ISingleInstance
    {
        private readonly DeviceTypeRepository _repository;
        public DeviceTypeService(DeviceTypeRepository repository) {
            _repository = repository;
        }
        public async Task<ApiResult<bool>> Add(DeviceTypeModel model)
        {
            var result = await _repository.Add(model);
            return ApiResult<bool>.Succeed(result);
        }

        public async Task<ApiResult<bool>> DeleteById(List<int> ids)
        {
            var result = await _repository.DeleteById(ids);
            return ApiResult<bool>.Succeed(result);
        }

        public async Task<ApiResult<DeviceTypeModel>> GetDeviceType(int id)
        {
            var result = await _repository.GetDeviceType(id);
            return ApiResult<DeviceTypeModel>.Succeed(result);
        }

        public async Task<ApiResult<List<DeviceTypeModel>>> GetDeviceTypeByCondition(DeviceTypeQuery query)
        {
            var result = await _repository.GetDeviceTypeByCondition(query);
            return ApiResult<List<DeviceTypeModel>>.Succeed(result);
        }

        public async Task<ApiResult<List<DeviceTypeModel>>> GetDeviceTypes()
        {
            var result = await _repository.GetDeviceTypes();
            return ApiResult<List<DeviceTypeModel>>.Succeed(result);
        }

        public async Task<ApiResult<Page<DeviceTypeModel>>> GetPageAsync(DeviceTypeQuery query)
        {
            var result = await _repository.GetPageAsync(query);
            return ApiResult<Page<DeviceTypeModel>>.Succeed(result);
        }

        public async Task<ApiResult<bool>> Modify(DeviceTypeModel model)
        {
            var result = await _repository.Modify(model);
            return ApiResult<bool>.Succeed(result);
        }

        public async Task<ApiResult<bool>> Validate(DeviceTypeModel model)
        {
            var message = "";
            if (!model.DeviceTypeCode.IsNullOrEmpty())
            {
                var result = await _repository.ExistsModelByCode(model.DeviceTypeCode);
                message = result ? "设备类型编码已存在" : message;
            }

            return message.IsNullOrEmpty() ? ApiResult<bool>.Succeed(true) : ApiResult<bool>.Failure(EnumReturnCode.CUSTOM_ERROR, message);
        }
    }
}

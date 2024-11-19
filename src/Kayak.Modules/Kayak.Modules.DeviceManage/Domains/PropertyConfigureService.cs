using Kayak.Core.Common.Enums;
using Kayak.Core.Common.Extensions;
using Kayak.Core.Common.Response;
using Kayak.DataAccess.DeviceData.Entities;
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
    public class PropertyConfigureService : ProxyServiceBase, IPropertyConfigureService, ISingleInstance
    {
        private readonly PropertyConfigureRepository _repository;
        public PropertyConfigureService(PropertyConfigureRepository repository)
        {
            _repository = repository;
        }
        public async Task<ApiResult<bool>> Add(PropertyConfigureModel model)
        {
            var result = await _repository.Add(model);
            return ApiResult<bool>.Succeed(result);
        }

        public async Task<ApiResult<bool>> DeleteById(List<int> ids)
        {
            var result = await _repository.DeleteById(ids);
            return ApiResult<bool>.Succeed(result);
        }

        public async Task<ApiResult<List<PropertyConfigureModel>>> GetListByDeviceIdAsync(string deviceId, List<string> propertyIds)
        {
            var result = await _repository.GetListByDeviceIdAsync(deviceId,propertyIds);
            return ApiResult<List<PropertyConfigureModel>>.Succeed(result);
        }

        public async Task<ApiResult<List<PropertyConfigureModel>>> GetListByIdAsync(List<int> ids)
        {
            var result = await _repository.GetListByIdAsync(ids);
            return ApiResult<List<PropertyConfigureModel>>.Succeed(result);
        }

        public async Task<ApiResult<PropertyConfigureModel>> GetByProperNameAsync(string correlationId, string correlationFrom, string propertyName)
        {
            var result = await _repository.GetByProperNameAsync(correlationId,correlationFrom,propertyName);
            return ApiResult<PropertyConfigureModel>.Succeed(result);
        }

        public async Task<ApiResult<Page<PropertyConfigureModel>>> GetPageAsync(PropertyConfigureQuery query)
        {
            var result = await _repository.GetPageAsync(query);
            return ApiResult<Page<PropertyConfigureModel>>.Succeed(result);
        }

        public async Task<ApiResult<bool>> Modify(PropertyConfigureModel model)
        {
            var result = await _repository.Modify(model);
            return ApiResult<bool>.Succeed(result);
        }

        public async Task<ApiResult<bool>> Validate(PropertyConfigureModel model)
        {
            var message = "";
            if (!model.PropertyId.IsNullOrEmpty())
            {
                var result = await _repository.ExistsModelById(model.Id, model.PropertyId,model.CorrelationFrom);
                message = result ? "属性标识已存在" : message;
            }
            if (model.PropertyName != null)
            {
                var result = await _repository.ExistsModelByName(model.Id, model.PropertyName, model.CorrelationFrom);
                message = result ? "属性名称已存在" : message;
            }
            return message.IsNullOrEmpty() ? ApiResult<bool>.Succeed(true) : ApiResult<bool>.Failure(EnumReturnCode.CUSTOM_ERROR, message);
        }
    }
}

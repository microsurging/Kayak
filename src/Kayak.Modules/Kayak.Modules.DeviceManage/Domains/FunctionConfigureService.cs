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

namespace Kayak.Modules.DeviceManage.Domains
{
    public class FunctionConfigureService : ProxyServiceBase, IFunctionConfigureService, ISingleInstance
    {
        private readonly FunctionConfigureRepository _repository;
        public FunctionConfigureService(FunctionConfigureRepository functionConfigureRepository) {
            _repository = functionConfigureRepository;
        }

        public async Task<ApiResult<bool>> Add(FunctionConfigureModel model)
        {
            var result = await _repository.Add(model);
            return ApiResult<bool>.Succeed(result);
        }

        public async Task<ApiResult<bool>> DeleteById(List<int> ids)
        {
            var result = await _repository.DeleteById(ids);
            return ApiResult<bool>.Succeed(result);
        }

        public async Task<ApiResult<Page<FunctionConfigureModel>>> GetPageAsync(FunctionConfigureQuery query)
        {
            var result = await _repository.GetPageAsync(query);
            return ApiResult<Page<FunctionConfigureModel>>.Succeed(result);
        }

        public async Task<ApiResult<bool>> Modify(FunctionConfigureModel model)
        {
            var result = await _repository.Modify(model);
            return ApiResult<bool>.Succeed(result);
        }

        public async Task<ApiResult<bool>> Validate(FunctionConfigureModel model)
        {
            var message = "";
            if (!model.FunctionId.IsNullOrEmpty())
            {
                var result = await _repository.ExistsModelById(model.Id, model.FunctionId, model.CorrelationFrom);
                message = result ? "功能标识已存在" : message;
            }
            if (model.FunctionName != null)
            {
                var result = await _repository.ExistsModelByName(model.Id, model.FunctionName, model.CorrelationFrom);
                message = result ? "功能名称已存在" : message;
            }
            return message.IsNullOrEmpty() ? ApiResult<bool>.Succeed(true) : ApiResult<bool>.Failure(EnumReturnCode.CUSTOM_ERROR, message);
        }
    }
}

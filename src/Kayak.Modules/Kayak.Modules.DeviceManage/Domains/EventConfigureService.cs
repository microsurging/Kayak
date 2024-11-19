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
    public class EventConfigureService : ProxyServiceBase, IEventConfigureService, ISingleInstance
    {
        private readonly EventConfigureRepository _repository;
        public EventConfigureService(EventConfigureRepository repository) {
            _repository = repository;
        }

        public async Task<ApiResult<bool>> Add(EventConfigureModel model)
        {
            var result = await _repository.Add(model);
            return ApiResult<bool>.Succeed(result);
        }

        public async Task<ApiResult<bool>> DeleteById(List<int> ids)
        {
            var result = await _repository.DeleteById(ids);
            return ApiResult<bool>.Succeed(result);
        }

        public async Task<ApiResult<Page<EventConfigureModel>>> GetPageAsync(EventConfigureQuery query)
        {
            var result = await _repository.GetPageAsync(query);
            return ApiResult<Page<EventConfigureModel>>.Succeed(result);
        }

        public async Task<ApiResult<bool>> Modify(EventConfigureModel model)
        {
            var result = await _repository.Modify(model);
            return ApiResult<bool>.Succeed(result);
        }

        public async Task<ApiResult<bool>> Validate(EventConfigureModel model)
        {
            var message = "";
            if (!model.EventId.IsNullOrEmpty())
            {
                var result = await _repository.ExistsModelById(model.Id, model.EventId, model.CorrelationFrom);
                message = result ? "事件标识已存在" : message;
            }
            if (model.EventName != null)
            {
                var result = await _repository.ExistsModelByName(model.Id, model.EventName, model.CorrelationFrom);
                message = result ? "事件名称已存在" : message;
            }
            return message.IsNullOrEmpty() ? ApiResult<bool>.Succeed(true) : ApiResult<bool>.Failure(EnumReturnCode.CUSTOM_ERROR, message);
        }
    }
}

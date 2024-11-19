using Kayak.Core.Common.Response;
using Kayak.IModuleServices.DeviceManage.Models;
using Kayak.IModuleServices.DeviceManage.Queries;
using Kayak.IModuleServices.DeviceManage;
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
    internal class EventParameterService : ProxyServiceBase, IEventParameterService, ISingleInstance
    {
        private readonly EventParameterRepository _repository;

        public EventParameterService(EventParameterRepository repository)
        {
            _repository = repository;
        }

        public async Task<ApiResult<bool>> Edit(string key, List<EventParameterModel> list)
        {
            var eventId = list.Select(p => p.EventId).FirstOrDefault();
            var propertyThresholds = await _repository.GetByEventId(eventId);
            var ids = list.Select(p => p.Id).ToList();
            var addList = list.Where(p => p.Id == null).ToList();
            var modifyList = list.Where(p => p.Id != null).ToList();
            await _repository.AddList(addList);
            foreach (var item in modifyList)
                await _repository.Modify(item);
            var delList = propertyThresholds.Where(p => !ids.Contains(p.Id));
            await _repository.DelBatch(delList.ToList());
            return ApiResult<bool>.Succeed(true);
        }

        public async Task<ApiResult<List<EventParameterModel>>> Get(EventParameterQuery query)
        {
            var result = await _repository.Get(query);
            return ApiResult<List<EventParameterModel>>.Succeed(result);
        }

        public async Task<ApiResult<List<EventParameterModel>>> GetByEventId(int eventId)
        {
            var result = await _repository.GetByEventId(eventId);
            return ApiResult<List<EventParameterModel>>.Succeed(result);
        }
    }
}

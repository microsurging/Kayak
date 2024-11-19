using Kayak.Core.Common.Repsitories;
using Kayak.Core.Common.Response;
using Kayak.DataAccess.DeviceData.Entities;
using Kayak.DataAccess.DeviceData;
using Kayak.IModuleServices.DeviceManage.Models;
using Kayak.IModuleServices.DeviceManage.Queries;
using Surging.Core.CPlatform.Ioc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Surging.Core.System.MongoProvider;
using Kayak.Core.Common.Extensions;
using Kayak.Core.Common.Context;

namespace Kayak.Modules.DeviceManage.Repositories
{
    public class EventConfigureRepository : BaseRepository, ISingleInstance
    { 
        private readonly IEFRepository<EventConfigure> _repository;
        public EventConfigureRepository(IEFRepository<EventConfigure> repository)
        { 
            _repository = repository;
        }

        public async Task<bool> Add(EventConfigureModel model)
        {
            using (var context = DataContext.Instance())
            {
                context.EventConfigure.Add(ToEntity(model));
                return await context.SaveChangesAsync() > 0;
            }
        }

        public Task<bool> DeleteById(List<int> ids)
        {
            using (var context = DataContext.Instance())
            {
                var result = _repository.Instance(context).ModifyBy(new EventConfigure { IsDeleted = true }, p => ids.Contains(p.Id), "IsDeleted") > 0;
                return Task.FromResult(result);
            }
        }

        public async Task<Page<EventConfigureModel>> GetPageAsync(EventConfigureQuery query)
        {
            using (var context = DataContext.Instance())
            {
                var result = new Page<EventConfigureModel>();
                var entities = await _repository.Instance(context).GetPageList(query.Page, query.PageSize, x => x
                .WhereIF(query.Id.HasValue, e => e.Id == query.Id)
                .WhereIF(!query.Ids.IsNullOrEmpty(), e => query.Ids.Contains(e.Id))
                .Where(e => e.CorrelationId == query.CorrelationId)
                .Where(e => e.CorrelationFrom == query.CorrelationFrom)
                .Where(e => e.IsDeleted == false)
                .OrderByDescending(e => e.Id)
                );
                result.Items = entities.Items.Select(p => ToModel(p)).ToList();
                result.Total = entities?.Total ?? 0;
                result.PageCount = entities?.PageCount ?? 0;
                result.PageIndex = entities?.PageIndex ?? 0;
                result.PageSize = entities?.PageSize ?? 0;
                return result;
            }
        }

        public Task<bool> Modify(EventConfigureModel model)
        {
            using (var context = DataContext.Instance())
            {
                var entity = ToEntity(model);
                var result = _repository.Instance(context).ModifyBy(entity, p => p.Id == model.Id, "EventId", "EventName", "Remark", "UpdateDate", "Eventlevel", "DataTypeValue", "Updater", "Expands") > 0;
                return Task.FromResult(result);
            }
        }

        public async Task<bool> ExistsModelById(int id, string eventId, string correlationFrom)
        {
            using (var context = DataContext.Instance())
            {
                return await context.EventConfigure.AsNoTracking().AnyAsync(p => p.Id != id && p.EventId == eventId && p.IsDeleted == false && p.CorrelationFrom == correlationFrom);
            }
        }

        public async Task<bool> ExistsModelByName(int id, string eventName, string correlationFrom)
        {
            using (var context = DataContext.Instance())
            {
                return await context.EventConfigure.AsNoTracking().AnyAsync(p => p.Id != id && p.EventName == eventName && p.IsDeleted == false && p.CorrelationFrom == correlationFrom);
            }
        }

        public EventConfigureModel ToModel(EventConfigure entity)
        {
            return entity == null ? default : new EventConfigureModel
            {
                Id = entity.Id,
                EventId = entity.EventId,
                DataTypeValue = entity.DataTypeValue,
                EventName = entity.EventName,
                Expands = entity.Expands,
                Eventlevel = entity.Eventlevel,
                CorrelationFrom = entity.CorrelationFrom,
                CorrelationId = entity.CorrelationId,
                CreateDate = entity.CreateDate,
                Remark = entity.Remark,
                UpdateDate = entity.UpdateDate,
            };
        }

        public EventConfigure ToEntity(EventConfigureModel model)
        {
            return model == null ? default : new EventConfigure
            {
                Id = model.Id,
                EventId = model.EventId,
                DataTypeValue = model.DataTypeValue,
                EventName = model.EventName,
                Expands = model.Expands,
                Eventlevel = model.Eventlevel,
                CorrelationFrom = model.CorrelationFrom,
                CorrelationId = model.CorrelationId,
                CreateDate = model.CreateDate,
                Remark = model.Remark,
                UpdateDate = model.UpdateDate,
                Creater = IdentityContext.Get()?.UserId,
                Updater = IdentityContext.Get()?.UserId,
            };
        }
    }
}
 
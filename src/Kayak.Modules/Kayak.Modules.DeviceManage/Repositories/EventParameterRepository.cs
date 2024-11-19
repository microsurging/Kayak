using Kayak.Core.Common.Context;
using Kayak.Core.Common.Extensions;
using Kayak.Core.Common.Repsitories;
using Kayak.DataAccess.DeviceData;
using Kayak.DataAccess.DeviceData.Entities;
using Kayak.IModuleServices.DeviceManage.Models;
using Kayak.IModuleServices.DeviceManage.Queries;
using Microsoft.EntityFrameworkCore;
using Surging.Core.CPlatform.Ioc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kayak.Modules.DeviceManage.Repositories
{
    public class EventParameterRepository : BaseRepository, ISingleInstance
    {
        private readonly IEFRepository<EventParameter> _repository;
        public EventParameterRepository(IEFRepository<EventParameter> repository)
        {
            _repository = repository;
        }

        public async Task<bool> AddList(List<EventParameterModel> model)
        {
            using (var context = DataContext.Instance())
            {
                context.EventParameter.AddRange(model.Select(p => ToEntity(p)).ToArray());
                return await context.SaveChangesAsync() > 0;
            }
        }

        public async Task<List<EventParameterModel>> Get(EventParameterQuery query)
        {
            using (var context = DataContext.Instance())
            {
                var queryable = context.EventParameter.Where(p => p.EventCode == query.EventCode);
                if (!query.ProductCode.IsNullOrEmpty())
                {
                    queryable = queryable.Where(p => p.ProductCode == query.ProductCode);
                }
                if (!query.DeviceCode.IsNullOrEmpty())
                {
                    queryable = queryable.Where(p => p.DeviceCode == query.DeviceCode);
                }
                var entities = await queryable.ToListAsync();
                return entities.Select(p => ToModel(p)).ToList();
            }
        }

        public async Task<List<EventParameterModel>> GetByEventId(int EventId)
        {
            using (var context = DataContext.Instance())
            {
                var entities = await context.EventParameter.AsNoTracking().Where(p => p.EventId == EventId).ToListAsync();
                return entities.Select(p => ToModel(p)).ToList();
            }
        }

        public Task<bool> Modify(EventParameterModel model)
        {
            using (var context = DataContext.Instance())
            {
                var entity = ToEntity(model);
                var result = _repository.Instance(context).ModifyBy(entity, p => p.Id == model.Id, "Code", "ProductCode", "Name", "DeviceCode", "EventCode", "DataTypeValue", "Updater", "UpdateDate") > 0;
                return Task.FromResult(result);
            }
        }

        public async Task<bool> DelBatch(List<EventParameterModel> list)
        {
            using (var context = DataContext.Instance())
            {
                var ids = list.Select(p => p.Id).ToList();
                var result = await _repository.Instance(context).Delete(p => ids.Contains(p.Id)) > 0;
                return result;
            }
        }

        public EventParameterModel ToModel(EventParameter entity)
        {
            return entity == null ? default : new EventParameterModel
            {
                Id = entity.Id,
                ProductCode = entity.ProductCode,
                DeviceCode = entity.DeviceCode,
                Code = entity.Code,
                DataTypeValue = entity.DataTypeValue,
                EventCode = entity.EventCode,
                EventId = entity.EventId,
                Name = entity.Name, 
                CreateDate = entity.CreateDate,
                UpdateDate = entity.UpdateDate,
            };
        }

        public EventParameter ToEntity(EventParameterModel model)
        {
            return model == null ? default : new EventParameter
            {
                ProductCode = model.ProductCode,
                DeviceCode = model.DeviceCode,
                Code = model.Code,
                DataTypeValue = model.DataTypeValue,
                EventCode = model.EventCode,
                EventId = model.EventId,
                Name = model.Name, 
                CreateDate = model.CreateDate,
                UpdateDate = model.UpdateDate, 
                Creater = IdentityContext.Get()?.UserId,
                Updater = IdentityContext.Get()?.UserId,
            };
        }
    }
}

using Kayak.Core.Common.Repsitories;
using Kayak.Core.Common.Response;
using Kayak.DataAccess.DeviceData.Entities;
using Kayak.DataAccess.DeviceData;
using Surging.Core.CPlatform.Ioc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kayak.IModuleServices.DeviceManage.Models;
using Kayak.IModuleServices.DeviceManage.Queries;
using Kayak.Core.Common.Extensions;
using Microsoft.EntityFrameworkCore;
using Surging.Core.System.MongoProvider;
using Kayak.Core.Common.Context;

namespace Kayak.Modules.DeviceManage.Repositories
{
    public class PropertyConfigureRepository : BaseRepository, ISingleInstance
    { 
        private readonly IEFRepository<PropertyConfigure> _repository;
        public PropertyConfigureRepository(IEFRepository<PropertyConfigure> repository)
        { 
            _repository = repository;
        }

        public async Task<bool> Add(PropertyConfigureModel model)
        {
            using (var context = DataContext.Instance())
            {
                context.PropertyConfigure.Add(ToEntity(model));
                return await context.SaveChangesAsync() > 0;
            }
        }

        public async  Task<PropertyConfigureModel> GetByProperNameAsync(string correlationId, string correlationFrom, string propertyName)
        {
            using (var context = DataContext.Instance())
            {
                var entity=await (from q in context.PropertyConfigure where q.CorrelationId==correlationId && q.CorrelationFrom==correlationFrom && q.PropertyName==propertyName select q).FirstOrDefaultAsync();
                return ToModel(entity);
            }
        }


        public Task<bool> DeleteById(List<int> ids)
        {
            using (var context = DataContext.Instance())
            {
                var result = _repository.Instance(context).ModifyBy(new PropertyConfigure { IsDeleted = true }, p => ids.Contains(p.Id), "IsDeleted") > 0;
                return Task.FromResult(result);
            }
        }

        public async Task<Page<PropertyConfigureModel>> GetPageAsync(PropertyConfigureQuery query)
        {
            using (var context = DataContext.Instance())
            {
                var result = new Page<PropertyConfigureModel>();
                var entities = await _repository.Instance(context).GetPageList(query.Page, query.PageSize, x => x
                .WhereIF(query.Id.HasValue, e => e.Id == query.Id)
                .WhereIF(!query.Ids.IsNullOrEmpty(), e => query.Ids.Contains(e.Id))
                .WhereIF(!query.InputText.IsNullOrEmpty(), e => e.PropertyName.Contains(query.InputText))
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

       public async Task<List<PropertyConfigureModel>> GetListByIdAsync(List<int> ids)
        {
            using (var context = DataContext.Instance())
            {
                var entities = await context.PropertyConfigure.Where(p => ids.Contains(p.Id)).ToListAsync();
                return entities.Select(p => ToModel(p)).ToList();
            }
        }

       public  async Task<List<PropertyConfigureModel>> GetListByDeviceIdAsync(string deviceId, List<string> propertyIds)
        {
            using (var context = DataContext.Instance())
            {
                var entities = await context.PropertyConfigure.Where(p => propertyIds.Contains(p.PropertyId) && p.CorrelationId == deviceId && p.CorrelationFrom == "device").ToListAsync();
                return entities.Select(p => ToModel(p)).ToList();
            }
        }

        public Task<bool> Modify(PropertyConfigureModel model)
        {
            using (var context = DataContext.Instance())
            {
                var entity = ToEntity(model);
                var result = _repository.Instance(context).ModifyBy(entity, p => p.Id == model.Id, "Name", "Value", "Remark", "UpdateDate", "DataTypeValue", "Precision", "SourceId", "DefaultValue",
                    "ValueRange", "Step", "UnitValue", "MaxLength", "Updater", "ReadWrite") > 0;
                return Task.FromResult(result);
            }
        }

        public async Task<bool> ExistsModelById(int id, string propertyId, string correlationFrom)
        {
            using (var context = DataContext.Instance())
            {
                return await context.PropertyConfigure.AsNoTracking().AnyAsync(p => p.Id != id && p.PropertyId == propertyId && p.IsDeleted == false && p.CorrelationFrom == correlationFrom);
            }
        }

        public async Task<bool> ExistsModelByName(int id, string propertyName, string correlationFrom)
        {
            using (var context = DataContext.Instance())
            {
                return await context.PropertyConfigure.AsNoTracking().AnyAsync(p => p.Id != id && p.PropertyName == propertyName && p.IsDeleted == false && p.CorrelationFrom == correlationFrom);
            }
        }

        public PropertyConfigureModel ToModel(PropertyConfigure entity)
        {
            return entity == null ? default : new PropertyConfigureModel
            {
                Id = entity.Id,
                PropertyName = entity.PropertyName,
                CorrelationFrom=entity.CorrelationFrom,
                 CorrelationId = entity.CorrelationId,
                PropertyId = entity.PropertyId,
                CreateDate = entity.CreateDate, 
                Precision=entity.Precision,
                SourceValue = entity.SourceValue,
                Remark = entity.Remark,
                Step = entity.Step,
                 ReadWrite=entity.ReadWrite,
                  ReadWriteType=entity.ReadWrite.Split("|").ToList(),
                DataTypeValue = entity.DataTypeValue,
                DefaultValue = entity.DefaultValue,
                UnitValue = entity.UnitValue, 
                ValueRange = entity.ValueRange,
                MaxLength=entity.MaxLength,
                UpdateDate = entity.UpdateDate,
            };
        }

        public PropertyConfigure ToEntity(PropertyConfigureModel model)
        {
            return model == null ? default : new PropertyConfigure
            {
                PropertyName = model.PropertyName,
                CorrelationFrom=model.CorrelationFrom,
                 CorrelationId=model.CorrelationId,
                PropertyId = model.PropertyId,
                CreateDate = model.CreateDate, 
                Remark = model.Remark,
                ReadWrite =string.Join("|", model.ReadWriteType),
                Step = model.Step,
                DataTypeValue = model.DataTypeValue,
                DefaultValue = model.DefaultValue,
                UnitValue = model.UnitValue, 
                 Precision = model.Precision,
                 MaxLength=model.MaxLength,
                SourceValue = model.SourceValue, 
                ValueRange = model.ValueRange,
                UpdateDate = model.UpdateDate, 
                Creater = IdentityContext.Get()?.UserId,
                Updater = IdentityContext.Get()?.UserId,
            };
        }
    }
}

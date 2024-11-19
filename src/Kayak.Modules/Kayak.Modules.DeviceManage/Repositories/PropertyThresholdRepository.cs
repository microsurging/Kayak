using Kayak.Core.Common.Repsitories;
using Kayak.DataAccess.DeviceData.Entities;
using Kayak.DataAccess.DeviceData;
using Kayak.IModuleServices.DeviceManage.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Surging.Core.CPlatform.Ioc;
using Kayak.Core.Common.Response;
using Microsoft.EntityFrameworkCore;
using Kayak.IModuleServices.DeviceManage.Queries;
using Kayak.Core.Common.Extensions;
using Kayak.Core.Common.Context;

namespace Kayak.Modules.DeviceManage.Repositories
{
    public class PropertyThresholdRepository : BaseRepository, ISingleInstance
    {
        private readonly IEFRepository<PropertyThreshold> _repository;
        public PropertyThresholdRepository(IEFRepository<PropertyThreshold> repository)
        {
            _repository = repository;
        }

        public async Task<bool> AddList(List<PropertyThresholdModel> model)
        {
            using (var context = DataContext.Instance())
            {
                context.PropertyThreshold.AddRange(model.Select(p=>ToEntity(p)).ToArray());
                return await context.SaveChangesAsync() > 0;
            }
        }

        public async  Task<List<PropertyThresholdModel>> Get(PropertyThresholdQuery query)
        {
            using (var context = DataContext.Instance())
            {
                var queryable = context.PropertyThreshold.Where(p=>p.PropertyCode==query.PropertyCode);
                if(!query.ProductCode.IsNullOrEmpty())
                {
                    queryable= queryable.Where(p=>p.ProductCode==query.ProductCode);
                }
                if (!query.DeviceCode.IsNullOrEmpty())
                {
                    queryable = queryable.Where(p => p.DeviceCode == query.DeviceCode);
                }
                var entities =await queryable.ToListAsync();
                return entities.Select(p=>ToModel(p)).ToList();
            }
        }

       public  async Task<List<PropertyThresholdModel>> GetByPropertyId(int propertyId)
        {
            using (var context = DataContext.Instance())
            {
                var entities=await context.PropertyThreshold.AsNoTracking().Where(p=>p.PropertyId==propertyId).ToListAsync();
                return entities.Select(p=>ToModel(p)).ToList();
            }
        }

        public Task<bool> Modify(PropertyThresholdModel model)
        {
            using (var context = DataContext.Instance())
            {
                var entity = ToEntity(model);
                var result = _repository.Instance(context).ModifyBy(entity, p => p.Id == model.Id, "ThresholdLevel", "ProductCode", "DeviceCode", "PropertyCode", "ThresholdType", "ThresholdValue", "Updater", "UpdateDate") > 0;
                return Task.FromResult(result);
            }
        }

        public async Task<bool> DelBatch(List<PropertyThresholdModel> list)
        {
            using (var context = DataContext.Instance())
            {
                var ids = list.Select(p =>p.Id).ToList();
                var result =await _repository.Instance(context).Delete(p=> ids.Contains(p.Id)) > 0;
                return result;
            }
        }

        public PropertyThresholdModel ToModel(PropertyThreshold entity)
        {
            return entity == null ? default : new PropertyThresholdModel
            {
                Id = entity.Id,
                PropertyId = entity.PropertyId,
                PropertyCode = entity.PropertyCode,
                 ProductCode = entity.ProductCode,
                ThresholdLevel = entity.ThresholdLevel,
                DeviceCode = entity.DeviceCode,
                ThresholdType = entity.ThresholdType,
                ThresholdValue = entity.ThresholdValue,
                CreateDate = entity.CreateDate,
                UpdateDate = entity.UpdateDate,
            };
        }

        public PropertyThreshold ToEntity(PropertyThresholdModel model)
        {
            return model == null ? default : new PropertyThreshold
            {
                ThresholdValue = model.ThresholdValue,
                PropertyId = model.PropertyId,
                PropertyCode=model.PropertyCode,
                  DeviceCode = model.DeviceCode,
                ThresholdType = model.ThresholdType,
                 ProductCode= model.ProductCode,
                ThresholdLevel = model.ThresholdLevel,
                UpdateDate = model.UpdateDate,
                CreateDate = model.CreateDate,
                 Creater = IdentityContext.Get()?.UserId,
                 Updater=IdentityContext.Get()?.UserId,
                    
            };
        }
    }
}

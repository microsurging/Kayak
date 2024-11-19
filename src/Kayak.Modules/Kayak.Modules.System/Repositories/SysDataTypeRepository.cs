using Kayak.Core.Common.Context;
using Kayak.Core.Common.Extensions;
using Kayak.Core.Common.Repsitories;
using Kayak.Core.Common.Response;
using Kayak.DataAccess.DeviceData;
using Kayak.DataAccess.DeviceData.Entities;
using Kayak.IModuleServices.System.Models;
using Kayak.IModuleServices.System.Queries;
using Microsoft.EntityFrameworkCore;
using Surging.Core.CPlatform.Ioc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kayak.Modules.System.Repositories
{
    public class SysDataTypeRepository : BaseRepository, ISingleInstance
    { 
        private readonly IEFRepository<SysDataType> _repository;
        public SysDataTypeRepository(IEFRepository<SysDataType> repository)
        { 
            _repository = repository;
        }

        public async Task<bool> Add(SysDataTypeModel model)
        {
            using (var context = DataContext.Instance())
            {
                context.SysDataType.Add(ToEntity(model));
                return await context.SaveChangesAsync() > 0;
            }
        }

        public Task<bool> DeleteById(List<int> ids)
        {
            using (var context = DataContext.Instance())
            {
                var result = _repository.Instance(context).ModifyBy(new SysDataType { IsDeleted = true }, p => ids.Contains(p.Id), "IsDeleted") > 0;
                return Task.FromResult(result);
            }
        }

        public async Task<Page<SysDataTypeModel>> GetPageAsync(SysDataTypeQuery query)
        {
            using (var context = DataContext.Instance())
            {
                var result = new Page<SysDataTypeModel>();
                var entities = await _repository.Instance(context).GetPageList(query.Page, query.PageSize, x => x
                .WhereIF(query.Id.HasValue, e => e.Id == query.Id)
                .WhereIF(!query.Ids.IsNullOrEmpty(), e => query.Ids.Contains(e.Id))
                .WhereIF(!query.Name.IsNullOrEmpty(), e => e.Name.Contains(query.Name))
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

        public Task<bool> Modify(SysDataTypeModel model)
        {
            using (var context = DataContext.Instance())
            {
                var entity = ToEntity(model);
                var result = _repository.Instance(context).ModifyBy(entity, p => p.Id == model.Id, "Name", "Value", "DefaultValue", "Remark", "Updater", "UpdateDate") > 0;
                return Task.FromResult(result);
            }
        }

        public async Task<List<SysDataTypeModel>> GetList()
        {
            using (var context = DataContext.Instance())
            {
                var entities = await (from q in context.SysDataType where q.IsDeleted == false select q).ToListAsync();
                return entities.Select(p => ToModel(p)).ToList();
            }
        }

        public async Task<bool> ExistsModelByName(int id, string name)
        {
            using (var context = DataContext.Instance())
            {
                return await context.SysDataType.AsNoTracking().AnyAsync(p => p.Id != id && p.Name == name && p.IsDeleted == false);
            }
        }

        public async Task<bool> ExistsModelByValue(int id, int? value)
        {
            using (var context = DataContext.Instance())
            {
                return await context.SysDataType.AsNoTracking().AnyAsync(p => p.Id != id && p.Value == value && p.IsDeleted == false);
            }
        }

        public SysDataTypeModel ToModel(SysDataType entity)
        {
            return entity == null ? default : new SysDataTypeModel
            {
                 Id=entity.Id,
                DefaultValue = entity.DefaultValue,
                Name = entity.Name,
                CreateDate = entity.CreateDate,
                UpdateDate = entity.UpdateDate,
                Remark = entity.Remark,
                Value = entity.Value, 
            };
        }

        public SysDataType ToEntity(SysDataTypeModel model)
        {
            return model == null ? default : new SysDataType
            {
                DefaultValue = model.DefaultValue,
                Name = model.Name,
                CreateDate = model.CreateDate,
                UpdateDate = model.UpdateDate,
                Remark = model.Remark,
                Value = model.Value,
                Updater = IdentityContext.Get()?.UserId,
                Creater = IdentityContext.Get()?.UserId,
            };
        }
    }
}

using Kayak.Core.Common.Repsitories;
using Kayak.Core.Common.Response; 
using Kayak.IModuleServices.System.Models;
using Kayak.IModuleServices.System.Queries;
using Microsoft.EntityFrameworkCore;
using Kayak.Core.Common.Extensions;
using Kayak.DataAccess.DeviceData;
using Kayak.DataAccess.DeviceData.Entities;
using Surging.Core.CPlatform.Ioc;
using Surging.Core.CPlatform.Utilities;
using System.Xml.Linq;
using Kayak.Core.Common.Context;

namespace Kayak.Modules.System.Repositories
{
    public class SysDictionaryRepository : BaseRepository, ISingleInstance
    { 
        private readonly IEFRepository<SysDictionary> _repository;
        public SysDictionaryRepository(IEFRepository<SysDictionary> repository)
        { 
            _repository = repository;
        }


        public async Task<Page<SysDictionaryModel>> GetPageAsync(SysDictionaryQuery query)
        {
            using (var context = DataContext.Instance())
            {
                var result = new Page<SysDictionaryModel>();
                var entities = await _repository.Instance(context).GetPageList(query.Page, query.PageSize, x => x
                .WhereIF(query.Id.HasValue, e => e.Id == query.Id)
                .WhereIF(!query.Ids.IsNullOrEmpty(), e => query.Ids.Contains(e.Id))
                .WhereIF(!query.Code.IsNullOrEmpty(), e => e.Code == query.Code)
                .WhereIF(query.ParentCode != null, e => e.ParentCode == query.ParentCode)
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

        public async Task<SysDictionaryModel> GetSysDictionary(string code)
        {
            using (var context = DataContext.Instance())
            {
                var entity = await context.SysDictionary.Where(p => p.Code == code).FirstOrDefaultAsync();
                return ToModel(entity);
            }
        }

        public async Task<List<SysDictionaryModel>> GetSysDictionaryByParentCode(string parentCode)
        {
            using (var context = DataContext.Instance())
            {
                var list = await context.SysDictionary.Where(p => p.ParentCode == parentCode).ToListAsync();
                return list.Select(p => ToModel(p)).ToList();
            }
        }

        public async Task<List<SysDictionaryModel>> GetSysDictionaryByCondition(SysDictionaryQuery query)
        {
            using (var context = DataContext.Instance())
            {
                var queryable = context.SysDictionary.AsNoTracking().Where(p => p.IsDeleted == false);
                if (query.ParentCode != null)
                {
                    queryable = queryable.Where(p => p.ParentCode == query.ParentCode);
                }

                if (query.Code != null)
                {
                    queryable = queryable.Where(p => p.Code == query.Code);
                }

                if (query.Name != null)
                {
                    queryable = queryable.Where(p => p.Name.Contains(query.Name));
                }
                var list = await queryable.ToListAsync();
                return list.Select(p => ToModel(p)).ToList();
            }
        }

        public async Task<bool> ExistsModelByName(int id, string parentCode, string name)
        {
            using (var context = DataContext.Instance())
            {
                if (string.IsNullOrEmpty(parentCode))
                    return await context.SysDictionary.AsNoTracking().AnyAsync(p => p.Id != id && p.Name == name && p.IsDeleted == false);
                else
                    return await context.SysDictionary.AsNoTracking().AnyAsync(p => p.ParentCode == parentCode && p.Name == name && p.IsDeleted == false);
            }
        }

        public async Task<bool> ExistsModelByCode(int id, string parentCode, string code)
        {
            using (var context = DataContext.Instance())
            {
                if (string.IsNullOrEmpty(parentCode))
                    return await context.SysDictionary.AsNoTracking().AnyAsync(p => p.Id != id && p.Code == code && p.IsDeleted == false);
                else
                    return await context.SysDictionary.AsNoTracking().AnyAsync(p => p.ParentCode == parentCode && p.Code == code && p.IsDeleted == false);
            }
        }

        public async Task<bool> ExistsModelByValue(string parentCode, int? value)
        {
            using (var context = DataContext.Instance())
            {
                return await context.SysDictionary.AsNoTracking().AnyAsync(p => p.ParentCode == parentCode && p.Value == value && p.IsDeleted == false);
            }
        }

        public async Task<bool> Add(SysDictionaryModel model)
        {
            using (var context = DataContext.Instance())
            {
                context.SysDictionary.Add(ToEntity(model));
                return await context.SaveChangesAsync() > 0;
            }
        }

        public  Task<bool> Modify(SysDictionaryModel model)
        {
            using (var context = DataContext.Instance())
            {
                var entity = ToEntity(model);
                var result = _repository.Instance(context).ModifyBy(entity, p => p.Id == model.Id, "Code", "Value", "IsShow", "Remark", "Updater", "UpdateDate") > 0;
                return Task.FromResult(result);
            }
        }

        public  Task<bool> DeleteById(List<int> ids)
        {
            using (var context = DataContext.Instance())
            {
                var result = _repository.Instance(context).ModifyBy(new SysDictionary { IsDeleted = true }, p => ids.Contains(p.Id), "IsDeleted") > 0;
                return Task.FromResult(result);
            }
        }

        public  Task<bool> Stop(List<int> ids)
        {
            using (var context = DataContext.Instance())
            {
                var result = _repository.Instance(context).ModifyBy(new SysDictionary { Status = 0 }, p => ids.Contains(p.Id), "Status") > 0;
                return Task.FromResult(result);
            }
        }

        public Task<bool> Open(List<int> ids)
        {
            using (var context = DataContext.Instance())
            {
                var result = _repository.Instance(context).ModifyBy(new SysDictionary { Status = 1 }, p => ids.Contains(p.Id), "Status") > 0;
                return Task.FromResult(result);
            }
        }


        public SysDictionaryModel ToModel(SysDictionary entity)
        { 
            return entity == null ? default : new SysDictionaryModel
            {
                 Id =entity.Id,
                Code = entity.Code,
                Name = entity.Name,
                CreateDate = entity.CreateDate,
                IsShow = entity.IsShow,
                ParentCode = entity.ParentCode,
                Status = entity.Status,
                UpdateDate = entity.UpdateDate,
                 Remark = entity.Remark,
                Value = entity.Value,
            }; 
        }

        public SysDictionary ToEntity(SysDictionaryModel model)
        { 
            return model == null ? default : new SysDictionary
            {
                Code = model.Code,
                Name = model.Name,
                CreateDate = model.CreateDate,
                IsShow = model.IsShow,
                ParentCode = model.ParentCode,
                Status = model.Status,
                UpdateDate = model.UpdateDate,
                 Remark = model.Remark,
                Value = model.Value, 
                Updater = IdentityContext.Get()?.UserId,
                Creater = IdentityContext.Get()?.UserId,
            }; 
        }
    }
}

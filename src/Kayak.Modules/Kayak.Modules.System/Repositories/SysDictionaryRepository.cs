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

namespace Kayak.Modules.System.Repositories
{
    public class SysDictionaryRepository : BaseRepository, ISingleInstance
    {
        private readonly DataContext _dataContext;
        private readonly IEFRepository<SysDictionary> _repository;
        public SysDictionaryRepository(IEFRepository<SysDictionary> repository)
        {
            _dataContext = ServiceLocator.GetService<DataContext>(AppConfig.DeviceDataOptions.DatabaseType.ToString());
            _repository = repository;
        }


        public async Task<Page<SysDictionaryModel>> GetPageAsync(SysDictionaryQuery query)
        {
            var result = new Page<SysDictionaryModel>();
            var entities = await _repository.Instance(_dataContext).GetPageList(query.Page, query.PageSize, x => x
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

        public async Task<SysDictionaryModel> GetSysDictionary(string code)
        {
            var entity = await _dataContext.SysDictionary.Where(p => p.Code == code).FirstOrDefaultAsync();
            return ToModel(entity);
        }

        public async Task<List<SysDictionaryModel>> GetSysDictionaryByParentCode(string parentCode)
        {
            var list = await _dataContext.SysDictionary.Where(p => p.ParentCode == parentCode).ToListAsync();
            return list.Select(p => ToModel(p)).ToList();
        }

        public async Task<List<SysDictionaryModel>> GetSysDictionaryByCondition(SysDictionaryQuery query)
        {
            var queryable = _dataContext.SysDictionary.AsNoTracking().Where(p => p.IsDeleted == false);
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

        public async Task<bool> ExistsModelByName(int id, string parentCode, string name)
        {
            if(string.IsNullOrEmpty(parentCode))
            return await _dataContext.SysDictionary.AsNoTracking().AnyAsync(p => p.Id != id && p.Name == name  && p.IsDeleted == false);
            else
                return await _dataContext.SysDictionary.AsNoTracking().AnyAsync(p => p.ParentCode == parentCode && p.Name == name && p.IsDeleted == false);
        }

        public async Task<bool> ExistsModelByCode(int id, string parentCode, string code)
        {
            if(string.IsNullOrEmpty(parentCode))
            return await _dataContext.SysDictionary.AsNoTracking().AnyAsync(p => p.Id != id && p.Code == code && p.IsDeleted == false);
            else
                return await _dataContext.SysDictionary.AsNoTracking().AnyAsync(p => p.ParentCode == parentCode && p.Code == code && p.IsDeleted == false);
        }

        public async Task<bool> ExistsModelByValue(string  parentCode, int? value)
        {
            return await _dataContext.SysDictionary.AsNoTracking().AnyAsync(p => p.ParentCode == parentCode && p.Value == value && p.IsDeleted == false);
        }

        public async Task<bool> Add(SysDictionaryModel model)
        {
            _dataContext.SysDictionary.Add(ToEntity(model));
            return await _dataContext.SaveChangesAsync() > 0;
        }
        public  Task<bool> Modify(SysDictionaryModel model)
        {
            var entity = ToEntity(model);
            var result = _repository.Instance(_dataContext).ModifyBy(entity, p => p.Id == model.Id,  "Code", "Value", "IsShow", "Remark", "UpdateDate") > 0;
            return Task.FromResult(result);
        }

        public  Task<bool> DeleteById(List<int> ids)
        {
          var result  = _repository.Instance(_dataContext).ModifyBy(new SysDictionary {  IsDeleted = true }, p=>ids.Contains(p.Id) ,"IsDeleted")>0;
            return Task.FromResult(result);
        }

        public  Task<bool> Stop(List<int> ids)
        { 
            var result = _repository.Instance(_dataContext).ModifyBy(new SysDictionary { Status = 0 }, p => ids.Contains(p.Id), "Status") > 0;
            return Task.FromResult(result);
        }

        public Task<bool> Open(List<int> ids)
        {
            var result = _repository.Instance(_dataContext).ModifyBy(new SysDictionary { Status = 1 }, p => ids.Contains(p.Id), "Status") > 0;
            return Task.FromResult(result);
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
                  
            }; 
        }
    }
}

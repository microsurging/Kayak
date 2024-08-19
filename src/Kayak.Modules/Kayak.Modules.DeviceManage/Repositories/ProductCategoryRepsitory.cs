using Kayak.Core.Common.Repsitories;
using Kayak.DataAccess.DeviceData.Entities;
using Kayak.DataAccess.DeviceData;
using Surging.Core.CPlatform.Ioc;
using Surging.Core.CPlatform.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kayak.IModuleServices.DeviceManage.Models;
using Kayak.Core.Common.Extensions;
using Kayak.Core.Common.Response;
using Kayak.IModuleServices.DeviceManage.Queries;
using Microsoft.EntityFrameworkCore;

namespace Kayak.Modules.DeviceManage.Repositories
{
    public  class ProductCategoryRepsitory : BaseRepository, ISingleInstance
    {
        private readonly DataContext _dataContext;
        private readonly IEFRepository<ProductCategory> _repository;
        public ProductCategoryRepsitory(IEFRepository<ProductCategory> repository)
        {
            _dataContext = ServiceLocator.GetService<DataContext>(AppConfig.DeviceDataOptions.DatabaseType.ToString());
            _repository = repository;
        }

        public async Task<bool> AddChildren(ProductCategoryModel model)
        {
            _repository.Instance(_dataContext).ModifyBy(new ProductCategory { IsChildren = true }, p =>p.CategoryId== model.CategoryId, "IsChildren");
            var id = await _dataContext.ProductCategory.MaxAsync(p => p.Id);
            model.CategoryId = model.CategoryId.IsNullOrEmpty() ? (++id).ToString() : $"{++id}-{model.CategoryId}";
            model.Level = model.CategoryId.Split("-").Length;
            await _dataContext.ProductCategory.AddAsync(ToEntity(model));

            return await _dataContext.SaveChangesAsync() > 0;
        }

        public async Task<bool> Add(ProductCategoryModel model)
        {
            
            var id = await _dataContext.ProductCategory.MaxAsync(p => p.Id);
            model.CategoryId = (++id).ToString();
            model.Level = model.CategoryId.Split("-").Length;
            await _dataContext.ProductCategory.AddAsync(ToEntity(model));

            return await _dataContext.SaveChangesAsync() > 0;
        }

        public Task<bool> DeleteById(List<int> ids)
        {
            var result = _repository.Instance(_dataContext).ModifyBy(new ProductCategory { IsDeleted = true }, p => ids.Contains(p.Id), "IsDeleted") > 0;
            return Task.FromResult(result);
        }

        public async Task<Page<ProductCategoryModel>> GetPageAsync(ProductCategoryQuery query)
        {
            var result = new Page<ProductCategoryModel>();
            var entities = await _repository.Instance(_dataContext).GetPageList(query.Page, query.PageSize, x => x
            .WhereIF(query.Id.HasValue, e => e.Id == query.Id)
            .WhereIF(!query.Ids.IsNullOrEmpty(), e => query.Ids.Contains(e.Id))
            .WhereIF(!query.Name.IsNullOrEmpty(), e => e.CategoryName.Contains(query.Name))
            .WhereIF(!query.CategoryId.IsNullOrEmpty(), e => e.CategoryId.EndsWith(query.CategoryId))
            .WhereIF(query.Level != null, e => e.Level == query.Level) 
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

        public async Task<List<ProductCategoryModel>> GetProductCategoryByCondition(ProductCategoryQuery query)
        {
            var queryable = _dataContext.ProductCategory.AsNoTracking().Where(p => 1 == 1);
            if (query.CategoryId != null)
            {
                queryable = queryable.Where(p => p.CategoryId.EndsWith(query.CategoryId));
            }
             
            if (query.Level != null)
            {
                queryable = queryable.Where(p => p.Level == query.Level);
            }

            if (query.Name != null)
            {
                queryable = queryable.Where(p => p.CategoryName.Contains(query.Name));
            }
            if (!query.Code.IsNullOrEmpty())
            {
                queryable = queryable.Where(p => p.Code.Contains(query.Code));
            }
            var list = await queryable.ToListAsync();
            return list.Select(p => ToModel(p)).ToList();
        }

        public async Task<List<ProductCategoryModel>> GetProductCategoryByIds(List<int> ids)
        {
            var list = await _dataContext.ProductCategory.AsNoTracking().Where(p => ids.Contains(p.Id)).ToListAsync();
            return list.Select(p => ToModel(p)).ToList();
        }

        public async Task<ProductCategoryModel> GetProductCategory(int id)
        {
            var entity = await _dataContext.ProductCategory.AsNoTracking().Where(p => p.Id == id).FirstOrDefaultAsync();
            return ToModel(entity);
        }

        public async Task<bool> ExistsModelByCode(string categoryId,string code)
        {
            return await _dataContext.ProductCategory.AsNoTracking().AnyAsync(p =>p.Code == code && p.CategoryId!= categoryId) ;
        }

        public  Task<bool> Modify(ProductCategoryModel model)
        {
            var entity = ToEntity(model);
            var result = _repository.Instance(_dataContext).ModifyBy(entity, p => p.Id == model.Id, "Code", "CategoryName", "Remark", "UpdateDate") > 0;
            return Task.FromResult(result);
        }

        public ProductCategoryModel ToModel(ProductCategory entity)
        {

            return entity == null ? default : new ProductCategoryModel
            {
                Id=entity.Id,
                 IsChildren=entity.IsChildren,
                CategoryId = entity.CategoryId,
                CategoryName = entity.CategoryName,
                Code = entity.Code,
                CreateDate = entity.CreateDate,
                Level = entity.Level,
                Remark = entity.Remark,
                UpdateDate = entity.UpdateDate,
            };
        }

        public ProductCategory ToEntity(ProductCategoryModel model)
        {

            return model == null ? default : new ProductCategory
            {
                CategoryId = model.CategoryId,
                CategoryName = model.CategoryName,
                Code = model.Code,
                CreateDate = model.CreateDate,
                Level = model.Level,
                Remark = model.Remark,
                UpdateDate = model.UpdateDate,

            };
        }
    }
}

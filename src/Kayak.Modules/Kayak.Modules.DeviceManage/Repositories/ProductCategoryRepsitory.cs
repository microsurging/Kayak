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
using Kayak.Core.Common.Context;

namespace Kayak.Modules.DeviceManage.Repositories
{
    public  class ProductCategoryRepsitory : BaseRepository, ISingleInstance
    { 
        private readonly IEFRepository<ProductCategory> _repository;
        public ProductCategoryRepsitory(IEFRepository<ProductCategory> repository)
        { 
            _repository = repository;
        }

        public async Task<bool> AddChildren(ProductCategoryModel model)
        {
            using (var context = DataContext.Instance())
            {
                _repository.Instance(context).ModifyBy(new ProductCategory { IsChildren = true }, p => p.CategoryId == model.CategoryId, "IsChildren");
                var id = await context.ProductCategory.MaxAsync(p => p.Id);
                model.CategoryId = model.CategoryId.IsNullOrEmpty() ? (++id).ToString() : $"{++id}-{model.CategoryId}";
                model.Level = model.CategoryId.Split("-").Length;
                await context.ProductCategory.AddAsync(ToEntity(model)); 
                return await context.SaveChangesAsync() > 0;
            }
        }

        public async Task<bool> Add(ProductCategoryModel model)
        {
            using (var context = DataContext.Instance())
            {
                var id = await context.ProductCategory.MaxAsync(p => p.Id);
                model.CategoryId = (++id).ToString();
                model.Level = model.CategoryId.Split("-").Length;
                await context.ProductCategory.AddAsync(ToEntity(model)); 
                return await context.SaveChangesAsync() > 0;
            }
        }

        public  async Task<List<ProductCategoryModel>> GetLastChild()
        {
            using (var context = DataContext.Instance())
            {
                var list = await context.ProductCategory.AsNoTracking().Where(p => p.IsChildren == false && p.IsDeleted == false).ToListAsync();
                return list.Select(p => ToModel(p)).ToList();
            }
        }

        public Task<bool> DeleteById(List<int> ids)
        {
            using (var context = DataContext.Instance())
            {
                var result = _repository.Instance(context).ModifyBy(new ProductCategory { IsDeleted = true }, p => ids.Contains(p.Id), "IsDeleted") > 0;
                return Task.FromResult(result);
            }
        }

        public async Task<Page<ProductCategoryModel>> GetPageAsync(ProductCategoryQuery query)
        {
            using (var context = DataContext.Instance())
            {
                var result = new Page<ProductCategoryModel>();
                var entities = await _repository.Instance(context).GetPageList(query.Page, query.PageSize, x => x
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
        }

        public async Task<List<ProductCategoryModel>> GetProductCategoryByCondition(ProductCategoryQuery query)
        {
            using (var context = DataContext.Instance())
            {
                var queryable = context.ProductCategory.AsNoTracking().Where(p => 1 == 1);
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
        }

        public async Task<List<ProductCategoryModel>> GetProductCategoryByIds(List<int> ids)
        {
            using (var context = DataContext.Instance())
            {
                var list = await context.ProductCategory.AsNoTracking().Where(p => ids.Contains(p.Id)).ToListAsync();
                return list.Select(p => ToModel(p)).ToList();
            }
        }

        public async Task<ProductCategoryModel> GetProductCategory(int id)
        {
            using (var context = DataContext.Instance())
            {
                var entity = await context.ProductCategory.AsNoTracking().Where(p => p.Id == id).FirstOrDefaultAsync();
                return ToModel(entity);
            }
        }

        public async Task<bool> ExistsModelByCode(string categoryId, string code)
        {
            using (var context = DataContext.Instance())
            {
                return await context.ProductCategory.AsNoTracking().AnyAsync(p => p.Code == code && p.CategoryId != categoryId);
            }
        }

        public Task<bool> Modify(ProductCategoryModel model)
        {
            using (var context = DataContext.Instance())
            {
                var entity = ToEntity(model);
                var result = _repository.Instance(context).ModifyBy(entity, p => p.Id == model.Id, "Code", "CategoryName", "Remark", "Updater", "UpdateDate") > 0;
                return Task.FromResult(result);
            }
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
                Creater = IdentityContext.Get()?.UserId,
                Updater = IdentityContext.Get()?.UserId,
            };
        }
    }
}

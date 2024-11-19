using Kayak.Core.Common.Repsitories;
using Kayak.Core.Common.Response; 
using Kayak.IModuleServices.DeviceManage.Models;
using Kayak.IModuleServices.DeviceManage.Queries;
using Kayak.Core.Common.Extensions;
using Microsoft.EntityFrameworkCore;
using Surging.Core.CPlatform.Ioc;
using Kayak.DataAccess.DeviceData;
using Kayak.DataAccess.DeviceData.Entities;
using Surging.Core.CPlatform.Utilities;
using Kayak.Core.Common.Context;

namespace Kayak.Modules.DeviceManage.Repositories
{
    public class ProductRepository : BaseRepository, ISingleInstance
    {
        private readonly IEFRepository<Product> _repository; 
        public ProductRepository(IEFRepository<Product> repository) {
            _repository = repository; 
        }

        public async Task<bool> Add(ProductModel model)
        {
            using (var context = DataContext.Instance())
            {
                context.Product.Add(ToEntity(model));
                return await context.SaveChangesAsync() > 0;
            }

        }

        public async Task<Page<ProductModel>> GetPageAsync(ProductQuery query)
        {
            using (var context = DataContext.Instance())
            {
                var result = new Page<ProductModel>();
                var entities = await _repository.Instance(context).GetPageList(query.Page, query.PageSize, x => x
                .WhereIF(query.Id.HasValue, e => e.Id == query.Id)
                .WhereIF(!query.Ids.IsNullOrEmpty(), e => query.Ids.Contains(e.Id))
                .WhereIF(query.CategoryId != null, e => e.CategoryId == query.CategoryId)
                .WhereIF(query.OrganizationId != null, e => e.OrganizationId == query.OrganizationId)
                 .WhereIF(query.DeviceType != null, e => e.DeviceType == query.DeviceType)
                   .WhereIF(!query.ProductCode.IsNullOrEmpty(), e => e.ProductCode == query.ProductCode)
                    .WhereIF(query.Protocol != null, e => e.Protocol == query.Protocol)
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

        public async Task<ProductStatisticsModel> GetProductStatistics()
        {
            using (var context = DataContext.Instance())
            {
                var model = new ProductStatisticsModel();
                model.NormalCount = await (from q in context.Product where q.Status == 1 select q.Id)
                              .CountAsync();
                model.DisableCount = await (from q in context.Product where q.Status == 0 select q.Id)
                            .CountAsync();
                return model;
            }
        }

        public async Task<bool> ExistsModelByCode(string code)
        {
            using (var context = DataContext.Instance())
            {
                return await context.Product.AsNoTracking().AnyAsync(p => p.ProductCode == code);
            }
        }

        public async Task<bool> DeleteById(List<int> ids)
        {
            using (var context = DataContext.Instance())
            {
                var result = _repository.Instance(context).ModifyBy(new Product { IsDeleted = true }, p => ids.Contains(p.Id), "IsDeleted") > 0;
                return await Task.FromResult(result);
            }
        }

        public async Task<bool> Stop(List<int> ids)
        {
            using (var context = DataContext.Instance())
            {
                var result = _repository.Instance(context).ModifyBy(new Product { Status = 0 }, p => ids.Contains(p.Id), "Status") > 0;
                return await Task.FromResult(result);
            }
        }

        public async Task<bool> Open(List<int> ids)
        {
            using (var context = DataContext.Instance())
            {
                var result = _repository.Instance(context).ModifyBy(new Product { Status = 1 }, p => ids.Contains(p.Id), "Status") > 0;
                return await Task.FromResult(result);
            }
        }

        public async Task<bool> Modify(ProductModel model)
        {
            using (var context = DataContext.Instance())
            {
                var entity = ToEntity(model);
                var result = _repository.Instance(context).ModifyBy(entity, p => p.Id == model.Id, "ProductCode", "CategoryId", "DeviceType", "Protocol", "OrganizationId", "Remark", "Updater", "UpdateDate") > 0;
                return await Task.FromResult(result);
            }
        }

        public async Task<List<ProductModel>> GetProductByCondition(ProductQuery query)
        {
            using (var context = DataContext.Instance())
            {
                var queryable = context.Product.AsNoTracking().Where(p => 1 == 1);
                if (query.ProductCode != null)
                {
                    queryable = queryable.Where(p => p.ProductCode.Contains(query.ProductCode));
                }

                if (query.Protocol != null)
                {
                    queryable = queryable.Where(p => p.Protocol == query.Protocol);
                }
                if (query.CategoryId != null)
                {
                    queryable = queryable.Where(p => p.CategoryId == query.CategoryId);
                }

                if (query.OrganizationId != null)
                {
                    queryable = queryable.Where(p => p.OrganizationId == query.OrganizationId);
                }

                if (query.DeviceType != null)
                {
                    queryable = queryable.Where(p => p.DeviceType == query.DeviceType);
                }
                var list = await queryable.ToListAsync();
                return list.Select(p => ToModel(p)).ToList();
            }
        }

        public async Task<List<ProductModel>> GetProducts()
        {
            using (var context = DataContext.Instance())
            {
                var list = await context.Product.Where(p => p.Status == 1 && p.IsDeleted == false).ToListAsync();
                return list.Select(p => ToModel(p)).ToList();
            }
        }

        public async Task<ProductModel> GetProduct(int id)
        {
            using (var context = DataContext.Instance())
            {
                var entity = await context.Product.Where(p => p.Id == id).FirstOrDefaultAsync();
                return ToModel(entity);
            }
        }

        public ProductModel ToModel(Product entity)
        {

            return entity == null ? default : new ProductModel
            {
                 Id = entity.Id,
                CategoryId = entity.CategoryId,
                DeviceType = entity.DeviceType,
                CreateDate = entity.CreateDate,
                  IsDeleted = entity.IsDeleted,
                   Status = entity.Status,  
                ProductCode = entity.ProductCode,
                 ProductName = entity.ProductName,
                Protocol = entity.Protocol, 
                OrganizationId = entity.OrganizationId,
                UpdateDate = entity.UpdateDate,
                Remark = entity.Remark

            };
        }

        public Product ToEntity(ProductModel model)
        {

            return model == null ? default : new Product
            {
                 CategoryId = model.CategoryId,
                  DeviceType = model.DeviceType,
                 CreateDate = model.CreateDate,
                 ProductCode = model.ProductCode,
                 ProductName = model.ProductName,
                 Protocol = model.Protocol,
                 OrganizationId = model.OrganizationId,
                UpdateDate = model.UpdateDate,
                Remark=model.Remark,
                Creater = IdentityContext.Get()?.UserId,
                Updater = IdentityContext.Get()?.UserId,

            }; 
        }
    }
}

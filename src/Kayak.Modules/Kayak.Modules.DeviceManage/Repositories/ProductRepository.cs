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

namespace Kayak.Modules.DeviceManage.Repositories
{
    public class ProductRepository : BaseRepository, ISingleInstance
    {
        private readonly IEFRepository<Product> _repository;
        private readonly DataContext _dataContext;
        public ProductRepository(IEFRepository<Product> repository) {
            _repository = repository;
            _dataContext = DataContext.Instance();
        }

        public async Task<bool> Add(ProductModel model)
        { 
            _dataContext.Product.Add(ToEntity(model));
            return await _dataContext.SaveChangesAsync() > 0;

        }

        public async Task<Page<ProductModel>> GetPageAsync(ProductQuery query)
        { 
            var result = new Page<ProductModel>();
            var entities = await _repository.Instance(_dataContext).GetPageList(query.Page, query.PageSize, x => x
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

        public async Task<bool> ExistsModelByCode(string code)
        { 
            return await _dataContext.Product.AsNoTracking().AnyAsync(p => p.ProductCode == code);
        }

        public async Task<bool> DeleteById(List<int> ids)
        { 
            var result = _repository.Instance(_dataContext).ModifyBy(new Product { IsDeleted = true }, p => ids.Contains(p.Id), "IsDeleted") > 0;
            return await Task.FromResult(result);
        }

        public async Task<bool> Stop(List<int> ids)
        { 
            var result = _repository.Instance(_dataContext).ModifyBy(new Product { Status = 0 }, p => ids.Contains(p.Id), "Status") > 0;
            return await Task.FromResult(result);
        }

        public async Task<bool> Open(List<int> ids)
        { 
            var result = _repository.Instance(_dataContext).ModifyBy(new Product { Status = 1 }, p => ids.Contains(p.Id), "Status") > 0;
            return await Task.FromResult(result);
        }

        public async Task<bool> Modify(ProductModel model)
        { 
            var entity = ToEntity(model);
            var result = _repository.Instance(_dataContext).ModifyBy(entity, p => p.Id == model.Id, "ProductCode", "CategoryId", "DeviceType", "Protocol", "OrganizationId", "Remark", "UpdateDate") > 0;
            return await Task.FromResult(result);
        }

        public async Task<List<ProductModel>> GetProductByCondition(ProductQuery query)
        { 
            var queryable = _dataContext.Product.AsNoTracking().Where(p => 1 == 1);
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

        public async Task<List<ProductModel>> GetProducts()
        {
            var list = await _dataContext.Product.Where(p=>p.Status==1 && p.IsDeleted==false).ToListAsync();
            return list.Select(p => ToModel(p)).ToList();
        }

        public async Task<ProductModel> GetProduct(int id)
        { 
            var entity = await _dataContext.Product.Where(p => p.Id == id).FirstOrDefaultAsync();
            return ToModel(entity);
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
                Remark=model.Remark

            }; 
        }
    }
}

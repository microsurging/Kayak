using Kayak.Core.Common;
using Kayak.Core.Common.Response;
using Kayak.IModuleServices.Aggregation.DeviceManage;
using Kayak.IModuleServices.Aggregation.DeviceManage.Models;
using Kayak.IModuleServices.DeviceManage.Models;
using Kayak.IModuleServices.DeviceManage.Queries;
using Surging.Core.CPlatform.Ioc;
using Surging.Core.ProxyGenerator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Kayak.Modules.Aggregation.DeviceManage.Domains
{
    public class PrdAggregationService : ProxyServiceBase, IPrdAggregationService, ISingleInstance
    {
        public async Task<ApiResult<Page<PrdAggregationModel>>> GetPageAsync(ProductQuery query)
        {

            var apiResult = await GetService<IModuleServices.DeviceManage.IProductService>().GetPageAsync(query);
            var categoryIds = apiResult.Result.Items.Select(p => p.CategoryId).ToList();
            var categories = await GetService<IModuleServices.DeviceManage.IProductCategoryService>().GetProductCategoryByIds(categoryIds);
            var protocols = await GetService<IModuleServices.System.ISysDictionaryService>().GetSysDictionaryByParentCode(SysDictionaryCodes.PROTOCOL);
            var deviceTypes = await GetService<IModuleServices.System.ISysDictionaryService>().GetSysDictionaryByParentCode( SysDictionaryCodes.DEVICETYPE);
            Page<PrdAggregationModel> pageModel = new Page<PrdAggregationModel>()
            {
                PageCount = apiResult.Result.PageCount,
                PageIndex = apiResult.Result.PageIndex,
                PageSize = apiResult.Result.PageSize,
                Total = apiResult.Result.Total,
                Items = apiResult.Result.Items.Select(x => new PrdAggregationModel
                {
                    CategoryId = x.CategoryId,
                    CreateDate = x.CreateDate,
                    DeviceType = x.DeviceType,
                    Status= x.Status,
                    IsDeleted = x.IsDeleted,
                    ProductName = x.ProductName,
                    Id = x.Id,
                    OrganizationId = x.OrganizationId,
                    Protocol = x.Protocol,
                    Category = categories.Result.Where(p => p.Id == x.CategoryId).FirstOrDefault(),
                    PrdProtocol = protocols.Result.Where(p => p.Value == x.Protocol).Select(p => new PrdDictionary
                    {
                        Code = p.Code,
                        Name = p.Name,
                        Value = p.Value,
                    }).FirstOrDefault(),
                    PrdDeviceType = deviceTypes.Result.Where(p => p.Value == x.DeviceType).Select(p => new PrdDictionary
                    {
                        Code = p.Code,
                        Name = p.Name,
                        Value = p.Value,
                    }).FirstOrDefault(),
                    ProductCode = x.ProductCode,
                    Remark = x.Remark,
                    UpdateDate = x.UpdateDate,


                }).ToList()
            };
            return ApiResult<Page<PrdAggregationModel>>.Succeed(pageModel);
        }

        public async Task<ApiResult<List<ProductModel>>> GetProducts()
        {
           return await GetService<IModuleServices.DeviceManage.IProductService>().GetProducts();
        }
    }
}

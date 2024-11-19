using Kayak.Core.Common;
using Kayak.Core.Common.Response;
using Kayak.IModuleServices.Aggregation.DeviceManage;
using Kayak.IModuleServices.Aggregation.DeviceManage.Models;
using Kayak.IModuleServices.DeviceManage.Models;
using Kayak.IModuleServices.DeviceManage.Queries;
using Surging.Core.CPlatform.Ioc;
using Surging.Core.ProxyGenerator;
using Surging.Core.System.MongoProvider;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kayak.Modules.Aggregation.DeviceManage.Domains
{
    internal class DeviceTypeAggService : ProxyServiceBase, IDeviceTypeAggService, ISingleInstance
    {
        public async Task<ApiResult<Page<DeviceTypeAggregationModel>>> GetPageAsync(DeviceTypeQuery query)
        {
            var apiResult = await GetService<IModuleServices.DeviceManage.IDeviceTypeService>().GetPageAsync(query);
            var prdCategories = await GetService<IModuleServices.DeviceManage.IProductCategoryService>().GetLastChild();
            var orgs = await GetService<IModuleServices.System.ISysOrganizationService>().GetOrgCompany();
            var protocols = await GetService<IModuleServices.DeviceAccess.IProtocolManageService>().GetProtocols();
            Page<DeviceTypeAggregationModel> pageModel = new Page<DeviceTypeAggregationModel>()
            {
                PageCount = apiResult.Result.PageCount,
                PageIndex = apiResult.Result.PageIndex,
                PageSize = apiResult.Result.PageSize,
                Total = apiResult.Result.Total,
                Items = apiResult.Result.Items.Select(x => new DeviceTypeAggregationModel
                {
                    Id = x.Id,
                    Code= x.Code,
                    ConnProtocolCode = x.ConnProtocolCode,
                    CreateDate = x.CreateDate,
                    DeviceTypeCode = x.DeviceTypeCode,
                    DeviceTypeName = x.DeviceTypeName,
                    ProductCategoryId = x.ProductCategoryId,
                    ProtocolCode = x.ProtocolCode,
                    OrganizationId = x.OrganizationId,
                    UpdateDate = x.UpdateDate,
                    Remark = x.Remark,
                    Organization = protocols.Result.Where(p => p.Id == x.OrganizationId).Select(p => new DataDictionary
                    {
                        Code = p.ProtocolCode,
                        Name = p.ProtocolName,
                        Id = p.Id,
                    }).FirstOrDefault(),
                    ProductCategory = prdCategories.Result.Where(p => p.Id == x.ProductCategoryId).Select(p => new DataDictionary
                    {
                        Code = p.Code,
                        Name = p.CategoryName,
                        Id = p.Id,
                    }).FirstOrDefault(),
                    Protocol = protocols.Result.Where(p => p.ProtocolCode == x.ProtocolCode).Select(p => new DataDictionary
                    {
                        Code = p.ProtocolCode,
                        Name = p.ProtocolName,
                        Id = p.Id,
                    }).FirstOrDefault()
                }).ToList()
            };
            return ApiResult<Page<DeviceTypeAggregationModel>>.Succeed(pageModel);
        }

        public async Task<ApiResult<DeviceTypeAggregationModel>> GetDeviceTypeById(int id)
        {
            var apiResult = await GetService<IModuleServices.DeviceManage.IDeviceTypeService>().GetDeviceType(id);
            var deviceType = apiResult.Result;
            var prdCategories = await GetService<IModuleServices.DeviceManage.IProductCategoryService>().GetLastChild();
            var orgs = await GetService<IModuleServices.System.ISysOrganizationService>().GetOrgCompany();
            var protocols = await GetService<IModuleServices.DeviceAccess.IProtocolManageService>().GetProtocols();
            return ApiResult<DeviceTypeAggregationModel>.Succeed(new DeviceTypeAggregationModel
            {
                Id = deviceType.Id,
                Code = deviceType.Code,
                ConnProtocolCode = deviceType.ConnProtocolCode,
                CreateDate = deviceType.CreateDate,
                DeviceTypeCode = deviceType.DeviceTypeCode,
                DeviceTypeName = deviceType.DeviceTypeName,
                ProductCategoryId = deviceType.ProductCategoryId,
                ProtocolCode = deviceType.ProtocolCode,
                OrganizationId = deviceType.OrganizationId,
                UpdateDate = deviceType.UpdateDate,
                Remark = deviceType.Remark,
                Organization = protocols.Result.Where(p => p.Id == deviceType.OrganizationId).Select(p => new DataDictionary
                {
                    Code = p.ProtocolCode,
                    Name = p.ProtocolName,
                    Id = p.Id,
                }).FirstOrDefault(),
                ProductCategory = prdCategories.Result.Where(p => p.Id == deviceType.ProductCategoryId).Select(p => new DataDictionary
                {
                    Code = p.Code,
                    Name = p.CategoryName,
                    Id = p.Id,
                }).FirstOrDefault(),
                Protocol = protocols.Result.Where(p => p.ProtocolCode == deviceType.ProtocolCode).Select(p => new DataDictionary
                {
                    Code = p.ProtocolCode,
                    Name = p.ProtocolName,
                    Id = p.Id,
                }).FirstOrDefault()
            });
        }

        public async Task<ApiResult<List<DeviceTypeModel>>> GetDeviceTypes()
        {
            return await GetService<IModuleServices.DeviceManage.IDeviceTypeService>().GetDeviceTypes();
        }
    }
}

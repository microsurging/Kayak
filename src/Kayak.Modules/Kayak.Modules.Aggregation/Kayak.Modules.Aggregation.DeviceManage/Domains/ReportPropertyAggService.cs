using Kayak.Core.Common.Extensions;
using Kayak.Core.Common.Response;
using Kayak.IModuleServices.Aggregation.DeviceManage;
using Kayak.IModuleServices.Aggregation.DeviceManage.Models;
using Kayak.IModuleServices.DeviceManage.Queries;
using Surging.Core.CPlatform.Ioc;
using Surging.Core.ProxyGenerator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kayak.Modules.Aggregation.DeviceManage.Domains
{
    internal class ReportPropertyAggService : ProxyServiceBase, IReportPropertyAggService, ISingleInstance
    {
        public async Task<ApiResult<Page<ReportPropertyAggModel>>> GetPageAsync(ReportPropertyQuery query)
        {

            var apiResult = await GetService<IModuleServices.DeviceManage.IPropertyConfigureService>().GetPageAsync(new PropertyConfigureQuery
            {
                CorrelationFrom = "device",
                CorrelationId = query.DeviceId

            }); 
            if(!query.PropertyName.IsNullOrEmpty())
            {
                apiResult.Result.Items = apiResult.Result.Items.Where(p=>p.PropertyName==query.PropertyName).ToList();
            }
            var propertyIds = apiResult.Result.Items.Select(x => x.PropertyId).ToList();
            if(!query.PropertyId.IsNullOrEmpty())
            {
                propertyIds.Clear();
                propertyIds.Add(query.PropertyId);
                apiResult.Result.Items = apiResult.Result.Items.Where(p => p.PropertyId == query.PropertyId).ToList();
            }
            var reportProperties = await GetService<IModuleServices.DeviceManage.IReportPropertyService>().GetListByDeviceIds(query.DeviceId, propertyIds);
            Page<ReportPropertyAggModel> pageModel = new Page<ReportPropertyAggModel>()
            {
                PageCount = apiResult.Result.PageCount,
                PageIndex = apiResult.Result.PageIndex,
                PageSize = apiResult.Result.PageSize,
                Total = apiResult.Result.Total,
                Items = apiResult.Result.Items.Select(x => new ReportPropertyAggModel
                {
                    CreateDate = reportProperties?.Result.Where(p => p.PropertyId == x.PropertyId).Select(p => p.CreateDate).FirstOrDefault(),
                    PropertyValue = reportProperties?.Result.Where(p => p.PropertyId == x.PropertyId).Select(p => p.PropertyValue).FirstOrDefault(),
                     PropertyId = x.PropertyId,
                    DeviceId = x.CorrelationId,
                      UnitValue  = x.UnitValue,
                    PropertyName = x.PropertyName
                    
                }).ToList()
            };
            return ApiResult<Page<ReportPropertyAggModel>>.Succeed(pageModel); ;
        }

        public async Task<ApiResult<Page<ReportPropertyAggModel>>> GetReportPropertyPageAsync(ReportPropertyQuery query)
        {

            var apiResult = await GetService<IModuleServices.DeviceManage.IReportPropertyService>().GetPageAsync(query);
            var propertyIds = apiResult.Result.Items.Select(x => x.PropertyId).ToList();
            var properties = await GetService<IModuleServices.DeviceManage.IPropertyConfigureService>().GetListByDeviceIdAsync(query.DeviceId, propertyIds);
            Page<ReportPropertyAggModel> pageModel = new Page<ReportPropertyAggModel>()
            {
                PageCount = apiResult.Result.PageCount,
                PageIndex = apiResult.Result.PageIndex,
                PageSize = apiResult.Result.PageSize,
                Total = apiResult.Result.Total,
                Items = apiResult.Result.Items.Select(x => new ReportPropertyAggModel
                {
                     Id=x.Id,
                    CreateDate = x.CreateDate,
                    PropertyValue = x.PropertyValue,
                    PropertyId = x.PropertyId,
                    DeviceId = x.DeviceId,
                     UnitValue  = properties.Result.Where(p => p.PropertyId == x.PropertyId).Select(p => p.UnitValue).FirstOrDefault(),
                    PropertyName = properties.Result.Where(p => p.PropertyId == x.PropertyId).Select(p => p.PropertyName).FirstOrDefault()

                }).ToList()
            };
            return ApiResult<Page<ReportPropertyAggModel>>.Succeed(pageModel); ;
        }
    }
}

using Kayak.Core.Common.Extensions;
using Kayak.Core.Common.Response;
using Kayak.IModuleServices.Aggregation.System;
using Kayak.IModuleServices.Aggregation.System.Models;
using Kayak.IModuleServices.DeviceManage;
using Kayak.IModuleServices.System;
using Kayak.IModuleServices.System.Queries;
using Microsoft.AspNetCore.Http;
using Surging.Core.CPlatform.Ioc;
using Surging.Core.ProxyGenerator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Joins;
using System.Text;
using System.Threading.Tasks;

namespace Kayak.Modules.Aggregation.System.Domains
{
    public class ReportPropertyLogAggService : ProxyServiceBase, IReportPropertyLogAggService, ISingleInstance
    {
        public async Task<ApiResult<Page<ReportPropertyLogAggModel>>> GetPageAsync(Globalization? global, ReportPropertyLogQuery query)
        {

            if (!query.PropertyName.IsNullOrEmpty())
            {
                var propertyConfigResult = await GetService<IPropertyConfigureService>().GetByProperNameAsync(query.DeviceCode, "device", query.PropertyName);
                query.PropertyId = propertyConfigResult.Result?.Id??0;
            }
                    
            var apiResult = await GetService<IReportPropertyLogService>().GetPageAsync(query);
            var propertyIds = apiResult.Result.Items.Select(x => x.PropertyId).ToList();
            var propertyConfigs = await GetService<IPropertyConfigureService>().GetListByIdAsync(propertyIds);
            Page<ReportPropertyLogAggModel> pageModel = new Page<ReportPropertyLogAggModel>()
            {
                PageCount = apiResult.Result.PageCount,
                PageIndex = apiResult.Result.PageIndex,
                PageSize = apiResult.Result.PageSize,
                Total = apiResult.Result.Total,
                Items = apiResult.Result.Items.Select(x => new ReportPropertyLogAggModel
                {
                    Content = x.Content,
                    CreateDate = x.CreateDate,
                    DeviceCode = x.DeviceCode,
                    Id = x.Id,
                    Level = x.Level,
                    ProductCode = x.ProductCode,
                    PropertyId = x.PropertyId,
                    PropertyValue = x.PropertyValue,
                    ThresholdType = x.ThresholdType,
                    ThresholdValue = x.ThresholdValue,
                    Status = x.Status,
                    PropertyName = propertyConfigs.Result.Where(p => p.Id == x.PropertyId).Select(p => p.PropertyName).FirstOrDefault(),
                }).ToList()
            };
            foreach (var item in pageModel.Items)
            { 
                item.Duration = DateStringFromNow(item.CreateDate.Value);
                item.Reason = $"{item.PropertyName} = {item.PropertyValue}";
                if (global == null || global == Globalization.CN)
                {
                    switch (item.ThresholdType)
                    {
                        case "between":
                            item.ThresholdCondition = $"{item.PropertyName}不在{item.ThresholdValue}范围之间";
                            break;
                        case ">":
                            item.ThresholdCondition = $"{item.PropertyName}未大于{item.ThresholdValue}";
                            break;
                        case "<":
                            item.ThresholdCondition = $"{item.PropertyName}未小于{item.ThresholdValue}";
                            break;
                        case "=":
                            item.ThresholdCondition = $"{item.PropertyName}未小于{item.ThresholdValue}";
                            break;
                    }
                }
                else
                {
                    item.ThresholdCondition = $"not {item.PropertyName} {item.ThresholdType} {item.ThresholdValue}";
                }
            }

            return ApiResult<Page<ReportPropertyLogAggModel>>.Succeed(pageModel);
        }

        private string DateStringFromNow(DateTimeOffset dt)
        {
            TimeSpan span = DateTimeOffset.Now - dt;
            
             if (span.TotalDays > 1)
            {
                return $"{Math.Round(span.TotalDays,1)} d";
            }
            else if (span.TotalHours > 1)
            {
                return $"{Math.Round(span.TotalHours,1)} h";
            }
            else if (span.TotalMinutes > 1)
            {
                return $"{Math.Round(span.TotalMinutes,1)} min";
            }
            else
            {
                return $"{Math.Round(span.TotalSeconds,1)} sec";
            }
            
        }
    }
}

using Kayak.Core.Common.Response;
using Kayak.IModuleServices.Aggregation.DeviceManage;
using Kayak.IModuleServices.Aggregation.DeviceManage.Models;
using Kayak.IModuleServices.DeviceManage;
using Kayak.IModuleServices.DeviceManage.Models;
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
    public class DeviceMessageAggService : ProxyServiceBase, IDeviceMessageAggService, ISingleInstance
    {
        public async Task<ApiResult<DeviceMsgTotalModel>> GetDeviceMsgTotal()
        {
            DateTime now = DateTime.Now;
            DateTime startOfMonth = new DateTime(now.Year, now.Month, 1);
            DateTime endOfMonth = startOfMonth.AddMonths(1).AddDays(-1);
            var propMsgTotal_today= await GetService<IReportPropertyService>().GetStatistics(DateTime.Today, DateTime.Today.AddDays(1).AddMilliseconds(-1));
            var eventMsgTotal_today = await GetService<IDeviceEventService>().GetStatistics(DateTime.Today, DateTime.Today.AddDays(1).AddMilliseconds(-1));
            var propMsgTotal_month = await GetService<IReportPropertyService>().GetStatistics(startOfMonth, endOfMonth);
            var eventMsgTotal_month = await GetService<IDeviceEventService>().GetStatistics(startOfMonth, endOfMonth);

            return ApiResult<DeviceMsgTotalModel>.Succeed(new DeviceMsgTotalModel
            {
                TodayTotal = propMsgTotal_today.Result + eventMsgTotal_today.Result,
                MonthTotal = propMsgTotal_month.Result + eventMsgTotal_month.Result,
            });
        }

        public async Task<ApiResult<List<GroupStatistics>>> GetGroupStatistics(DateTime startDate, DateTime endDate)
        {
            var entity = new List<GroupStatistics>() { };
            var groupType = GroupType.Hour;
            var hour = (endDate - startDate).TotalHours;

            var day = (endDate - startDate).TotalDays;
            if(hour > 0 && hour<=1) {
                for (var i = 0; i <= 7; i++)
                {
                    var difmin = startDate.Minute - startDate.Minute % 10;
                    difmin = i == 0 ? difmin : difmin + i * 10;
                    entity.Add(new GroupStatistics
                    {
                        Date = startDate.Date.AddHours(startDate.Hour).AddMinutes(difmin)
                    });
                }
            }
            if (hour > 1 & hour <= 24)
            {
                groupType = GroupType.Day;
                for (var i = 0; i <= 8; i++)
                {
                    var difhour = startDate.Hour - startDate.Hour % 3;
                    difhour = i == 0 ? difhour : difhour + i * 3;
                    entity.Add(new GroupStatistics
                    {
                        Date = startDate.Date.AddHours(difhour)
                    });
                }
            }
            if (day > 1 & day <= 7)
            {
                groupType = GroupType.Week;
                for (var i = 0; i <= 7; i++)
                {
                    entity.Add(new GroupStatistics
                    {
                        Date = startDate.Date.AddDays(i)
                    });
                }
            }
            if (day > 7)
            {
                groupType = GroupType.Month; 
                var difmonth = startDate.Month;
                var step = 0;
                var date =startDate.Date;
                for (var i = 0; i <= 4; i++)
                {
                    var difDay = startDate.Day - startDate.Day % 7;
                    difDay = i == 0 ? difDay : difDay + i * 7;
                    var nextMonth = startDate.AddMonths(1);
                     var nextDay= nextMonth.AddDays(-nextMonth.Day).Day;

                    
                    if (nextDay < difDay)
                    {
                        difDay = 1;
                        step = i;
                        difmonth = nextMonth.Month;
                    }
                     date = new DateTime(startDate.Year, difmonth, difDay);
                    entity.Add(new GroupStatistics
                    {
                        Date = date
                    }) ;
                }

                if (endDate.Date > date)
                    entity.Add(new GroupStatistics
                    {
                        Date = endDate.Date
                    });
            }
            var propGroupStatistics = await GetService<IReportPropertyService>().GetGroupStatistics(new StatisticsQuery
            {
                StartDate = startDate,
                EndDate = endDate,
                GroupType = groupType,
            });
            var eventGroupStatistics = await GetService<IDeviceEventService>().GetGroupStatistics(new StatisticsQuery
            {
                StartDate = startDate,
                EndDate = endDate,
                GroupType = groupType,
            });
            return ApiResult<List<GroupStatistics>>.Succeed(
                propGroupStatistics?.Result.Concat(eventGroupStatistics?.Result).Concat(entity)
                .GroupBy(p=>p.Date)
                .Select(p=>new GroupStatistics { Date=p.Key, Count=p.Sum(m=>m.Count) })
                .OrderBy(p=>p.Date)
                .ToList());
        }
    }
}

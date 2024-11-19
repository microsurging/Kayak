using Kayak.Core.Common.Repsitories;
using Kayak.Core.Common.Response;
using Kayak.DataAccess.DeviceData.Entities;
using Kayak.DataAccess.DeviceData;
using Kayak.IModuleServices.DeviceManage.Models;
using Kayak.IModuleServices.DeviceManage.Queries;
using Surging.Core.CPlatform.Ioc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kayak.Core.Common.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.Internal;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Kayak.Core.Common.Context;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using Surging.Core.System.MongoProvider;
using System.Diagnostics;

namespace Kayak.Modules.DeviceManage.Repositories
{
    public class ReportPropertyRepository : BaseRepository, ISingleInstance
    { 
        private readonly IEFRepository<ReportProperty> _repository;
        public ReportPropertyRepository(IEFRepository<ReportProperty> repository)
        { 
            _repository = repository;
        }

        public async Task<bool> Add(ReportPropertyModel model)
        {
            using (var context = DataContext.Instance())
            {
                context.ReportProperty.Add(ToEntity(model));
                return await context.SaveChangesAsync() > 0;
            }
        }

        public Task<bool> DeleteById(List<int> ids)
        {
            using (var context = DataContext.Instance())
            {
                var result = _repository.Instance(context).ModifyBy(new ReportProperty { IsDeleted = true }, p => ids.Contains(p.Id), "IsDeleted") > 0;
                return Task.FromResult(result);
            }
        }

        public async Task<List<GroupStatistics>> GetGroupStatistics(StatisticsQuery query)
        {
            using (var context = DataContext.Instance())
            { 
                var result = new List<GroupStatistics>();
                switch (query.GroupType)
                {
                    case GroupType.Hour:
                        {
                            var entities = await context.ReportProperty.Where(q => DateTimeOffset.Compare(q.CreateDate.Value, query.StartDate) >= 0
                                                   && DateTimeOffset.Compare(q.CreateDate.Value, query.EndDate) <= 0)
                                                    .ToListAsync();
                            result = entities.GroupBy(q => new
                            {
                                Date = q.CreateDate.Value.Date,
                                Hour = q.CreateDate.Value.Hour,
                                Min= q.CreateDate.Value.Minute-q.CreateDate.Value.Minute%10,
                            }

                            ).Select(p =>
                                         new GroupStatistics
                                         {
                                             Date = p.Key.Date.AddHours(p.Key.Hour).AddMinutes(p.Key.Min),
                                             Count = p.Count()

                                         }).ToList();

                        }
                        break;
                    case GroupType.Day:
                        {
                            var entities = await context.ReportProperty.Where(q => DateTimeOffset.Compare(q.CreateDate.Value, query.StartDate) >= 0
                                                   && DateTimeOffset.Compare(q.CreateDate.Value, query.EndDate) <= 0)
                                                    .ToListAsync();
                            result = entities.GroupBy(q => new
                            {      
                                Hour = q.CreateDate.Value.Hour - q.CreateDate.Value.Hour % 3,
                                Date = q.CreateDate.Value.Date,
                 
                            }

                            ).Select(p =>
                                         new GroupStatistics
                                         {
                                             Date = p.Key.Date.AddHours(p.Key.Hour),
                                             Count = p.Count()

                                         }).ToList();

                        }
                        break;
                    case GroupType.Week:
                        {
                            var entities = await context.ReportProperty.Where(q => DateTimeOffset.Compare(q.CreateDate.Value, query.StartDate) >= 0
                                         && DateTimeOffset.Compare(q.CreateDate.Value, query.EndDate) <= 0)
                                          .ToListAsync();
                            result = entities.GroupBy(q => new
                            {
                                Date = q.CreateDate.Value.Date,
                            }).Select(p =>
                                         new GroupStatistics
                                         {
                                             Date = p.Key.Date,
                                             Count = p.Count()

                                         }).ToList();
                        }
                        break;
                    case GroupType.Month:
                        {
                            var entities = await context.ReportProperty.Where(q => DateTimeOffset.Compare(q.CreateDate.Value, query.StartDate) >= 0
                                         && DateTimeOffset.Compare(q.CreateDate.Value, query.EndDate) <= 0)
                                          .ToListAsync();
                            result = entities.GroupBy(q => new
                            {
                                Year = q.CreateDate.Value.Date.Year,
                                Month = q.CreateDate.Value.Date.Month,
                                Day = q.CreateDate.Value.Date.Day >= 0 && q.CreateDate.Value.Day < 7 ? 1 :
                                            q.CreateDate.Value.Date.Day >= 7 && q.CreateDate.Value.Date.Day < 14 ? 7 :
                                           q.CreateDate.Value.Date.Day >= 14 && q.CreateDate.Value.Date.Day < 21 ? 14 :
                                           q.CreateDate.Value.Date.Day >= 21 && q.CreateDate.Value.Date.Day < 28 ? 21 :
                                           q.CreateDate.Value.Date.Day >= 28 && q.CreateDate.Value.Date.Day <= 30 ? 30 : 31
                            }).Select(p =>
                            new GroupStatistics
                            {
                                Date = new DateTime(p.Key.Year, p.Key.Month, p.Key.Day),
                                Count = p.Count()

                            }).ToList();

                        }
                        break;
                }
                return result;

            }
        }

       public async Task<int> GetStatistics(DateTime startDate, DateTime endDate)
        {
            using (var context = DataContext.Instance())
            { 
                    var result = await (from q in context.ReportProperty
                                        where DateTimeOffset.Compare(q.CreateDate.Value, startDate) >= 0
                                        && DateTimeOffset.Compare(q.CreateDate.Value, endDate) <= 0
                                        select q).CountAsync();
                    return result;
                
            }
        }

        public async Task<Page<ReportPropertyModel>> GetPageAsync(ReportPropertyQuery query)
        {
            using (var context = DataContext.Instance())
            {
                var result = new Page<ReportPropertyModel>();
                var entities = await _repository.Instance(context).GetPageList(query.Page, query.PageSize, x => x
                .WhereIF(query.Id.HasValue, e => e.Id == query.Id)
                .WhereIF(!query.Ids.IsNullOrEmpty(), e => query.Ids.Contains(e.Id))
                .WhereIF(query.PropertyId != null, e => query.PropertyId == e.PropertyId)
                .WhereIF(!query.DeviceId.IsNullOrEmpty(), e => e.DeviceId == query.DeviceId)
                 .WhereIF(query.BeginDate != null && query.EndDate != null, e => DateTimeOffset.Compare(e.CreateDate.Value, query.BeginDate.Value) >= 0 && DateTimeOffset.Compare(e.CreateDate.Value, query.EndDate.Value) <= 0)
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

        public async Task<List<ReportPropertyModel>> GetListByDeviceIds(string deviceId, List<string> propertyIds)
        {
            using (var context = DataContext.Instance())
            {
                var list = await (from m in context.ReportProperty.AsNoTracking()
                                  where m.DeviceId == deviceId && propertyIds.Contains(m.PropertyId) && m.IsDeleted == false
                                  select m
                       ).ToListAsync() ?? new List<ReportProperty>();
                var result = (from q in list
                              join g in (from m in list
                                         group m by new { m.DeviceId, m.PropertyId }) on new { q.DeviceId, q.PropertyId, q.CreateDate } equals new { DeviceId = g.Key.DeviceId, PropertyId = g.Key.PropertyId, CreateDate = g.Max(j => j.CreateDate) }
                              select new ReportPropertyModel
                              {
                                  CreateDate = q.CreateDate,
                                  DeviceId = q.DeviceId,
                                  Id = q.Id,
                                  PropertyId = q.PropertyId,
                                  PropertyValue = q.PropertyValue,

                              }).ToList();
                return result;
            }
        }


        public ReportPropertyModel ToModel(ReportProperty entity)
        {
            return entity == null ? default : new ReportPropertyModel
            { 
                Id=entity.Id,
                CreateDate = entity.CreateDate,
                DeviceId = entity.DeviceId,
                PropertyValue = entity.PropertyValue,
                PropertyId = entity.PropertyId,
            };
        }

        public ReportProperty ToEntity(ReportPropertyModel model)
        {
            return model == null ? default : new ReportProperty
            {
              CreateDate = model.CreateDate,
               DeviceId = model.DeviceId,
                PropertyValue = model.PropertyValue,
                 PropertyId = model.PropertyId,
                   Creater= IdentityContext.Get()?.UserId
            };
        }
    }
}

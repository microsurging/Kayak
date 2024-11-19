using Kayak.Core.Common.Context;
using Kayak.Core.Common.Extensions;
using Kayak.Core.Common.Repsitories;
using Kayak.Core.Common.Response;
using Kayak.DataAccess.DeviceData;
using Kayak.DataAccess.DeviceData.Entities;
using Kayak.IModuleServices.System.Models;
using Kayak.IModuleServices.System.Queries;
using Microsoft.EntityFrameworkCore;
using Surging.Core.CPlatform.Ioc;
using Surging.Core.System.MongoProvider;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kayak.Modules.System.Repositories
{
    public  class ReportPropertyLogRepository : BaseRepository, ISingleInstance
    {
        private readonly IEFRepository<ReportPropertyLog> _repository;
        public ReportPropertyLogRepository(IEFRepository<ReportPropertyLog> repository)
        {
            _repository = repository;
        }

        public async Task<ReportPropertyLogModel> GetById(int id)
        {
            using (var context = DataContext.Instance())
            {
                var entity = await (from q in context.ReportPropertyLog where q.Id == id select q).FirstOrDefaultAsync();
                return ToModel(entity);
            }
        }

        public async Task<bool> DelBatch(List<int> ids)
        {
            using (var context = DataContext.Instance())
            {  
                var result = await _repository.Instance(context).Delete(p => ids.Contains(p.Id)) > 0;
                return result;
            }
        }
        public async Task<bool> Add(ReportPropertyLogModel model)
        {
            using (var context = DataContext.Instance())
            {
                context.ReportPropertyLog.Add(ToEntity(model));
                return await context.SaveChangesAsync() > 0;
            }
        }

        public async Task<Page<ReportPropertyLogModel>> GetPageAsync(ReportPropertyLogQuery query)
        {
            using (var context = DataContext.Instance())
            {
                var result = new Page<ReportPropertyLogModel>();
                var entities = await _repository.Instance(context).GetPageList(query.Page, query.PageSize, x => x
                .WhereIF(query.Id.HasValue, e => e.Id == query.Id)
                .WhereIF(!query.Ids.IsNullOrEmpty(), e => query.Ids.Contains(e.Id))
                .WhereIF(!query.DeviceCode.IsNullOrEmpty(), e => e.DeviceCode== query.DeviceCode)
                .WhereIF(!query.ProductCode.IsNullOrEmpty(),e=>e.ProductCode==query.ProductCode)
                .WhereIF(query.PropertyId!=null,e=>e.PropertyId==query.PropertyId)
                .WhereIF(!query.Level.IsNullOrEmpty(), e => e.Level == query.Level)
                 .WhereIF(query.StartTime != null && query.EndTime != null, e => DateTimeOffset.Compare(e.CreateDate.Value, query.StartTime.Value) >= 0 && DateTimeOffset.Compare(e.CreateDate.Value, query.EndTime.Value) <= 0)
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

        public ReportPropertyLogModel ToModel(ReportPropertyLog entity)
        {
            return entity == null ? default : new ReportPropertyLogModel
            {
                Id = entity.Id,
                Content = entity.Content,
                CreateDate = entity.CreateDate,
                Level = entity.Level,
                PropertyId = entity.PropertyId,
                 ThresholdValue = entity.ThresholdValue,
                 DeviceCode = entity.DeviceCode, 
                PropertyValue = entity.PropertyValue,
                Status = entity.Status,
                 ThresholdType = entity.ThresholdType,
                  ProductCode=entity.ProductCode,
                UpdateDate = entity.UpdateDate,
            }; 

        }

        public ReportPropertyLog ToEntity(ReportPropertyLogModel model)
        {
            return model == null ? default : new ReportPropertyLog
            {
                Id = model.Id,
                Content = model.Content,
                CreateDate = model.CreateDate,
                 ThresholdValue = model.ThresholdValue,
                Level = model.Level,
                 DeviceCode= model.DeviceCode,
                PropertyId = model.PropertyId, 
                PropertyValue = model.PropertyValue,
                Status = model.Status,
                 ProductCode = model.ProductCode,
                 ThresholdType = model.ThresholdType,
                UpdateDate = model.UpdateDate, 
                Updater = IdentityContext.Get()?.UserId,
                Creater = IdentityContext.Get()?.UserId,
            };
        }
    }
}

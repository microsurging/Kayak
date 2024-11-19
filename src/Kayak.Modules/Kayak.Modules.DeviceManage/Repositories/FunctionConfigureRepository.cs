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
using Kayak.Core.Common.Context;

namespace Kayak.Modules.DeviceManage.Repositories
{
    public class FunctionConfigureRepository : BaseRepository, ISingleInstance
    { 
        private readonly IEFRepository<FunctionConfigure> _repository;
        public FunctionConfigureRepository(IEFRepository<FunctionConfigure> repository)
        { 
            _repository = repository;
        }

        public async Task<bool> Add(FunctionConfigureModel model)
        {
            using (var context = DataContext.Instance())
            {
                context.FunctionConfigure.Add(ToEntity(model));
                return await context.SaveChangesAsync() > 0;
            }
        }

        public Task<bool> DeleteById(List<int> ids)
        {
            using (var context = DataContext.Instance())
            {
                var result = _repository.Instance(context).ModifyBy(new FunctionConfigure { IsDeleted = true }, p => ids.Contains(p.Id), "IsDeleted") > 0;
                return Task.FromResult(result);
            }
        }

        public async Task<Page<FunctionConfigureModel>> GetPageAsync(FunctionConfigureQuery query)
        {
            using (var context = DataContext.Instance())
            {
                var result = new Page<FunctionConfigureModel>();
                var entities = await _repository.Instance(context).GetPageList(query.Page, query.PageSize, x => x
                .WhereIF(query.Id.HasValue, e => e.Id == query.Id)
                .WhereIF(!query.Ids.IsNullOrEmpty(), e => query.Ids.Contains(e.Id))
                .Where(e => e.CorrelationId == query.CorrelationId)
                .Where(e => e.CorrelationFrom == query.CorrelationFrom)
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

        public Task<bool> Modify(FunctionConfigureModel model)
        {
            using (var context = DataContext.Instance())
            {
                var entity = ToEntity(model);
                var result = _repository.Instance(context).ModifyBy(entity, p => p.Id == model.Id, "FunctionId", "FunctionName", "Remark", "UpdateDate", "IsAsync", "InputIds", "Updater", "OutputIds") > 0;
                return Task.FromResult(result);
            }
        }

        public async Task<bool> ExistsModelById(int id, string functionId, string correlationFrom)
        {
            using (var context = DataContext.Instance())
            {
                return await context.FunctionConfigure.AsNoTracking().AnyAsync(p => p.Id != id && p.FunctionId == functionId && p.IsDeleted == false && p.CorrelationFrom == correlationFrom);
            }
        }

        public async Task<bool> ExistsModelByName(int id, string functionName, string correlationFrom)
        {
            using (var context = DataContext.Instance())
            {
                return await context.FunctionConfigure.AsNoTracking().AnyAsync(p => p.Id != id && p.FunctionName == functionName && p.IsDeleted == false && p.CorrelationFrom == correlationFrom);
            }
        }

        public FunctionConfigureModel ToModel(FunctionConfigure entity)
        {
            return entity == null ? default : new FunctionConfigureModel
            {
                Id = entity.Id,
                FunctionId = entity.FunctionId,
                FunctionName = entity.FunctionName,
                InputIds = entity.InputIds,
                 IsAsync= entity.IsAsync,
                OutputIds = entity.OutputIds,
                CorrelationFrom = entity.CorrelationFrom,
                CorrelationId = entity.CorrelationId,
                CreateDate = entity.CreateDate,
                Remark = entity.Remark,
                UpdateDate = entity.UpdateDate,
            };
        }

        public FunctionConfigure ToEntity(FunctionConfigureModel model)
        {
            return model == null ? default : new FunctionConfigure
            {
                FunctionId = model.FunctionId,
                FunctionName = model.FunctionName,
                InputIds = model.InputIds,
                OutputIds = model.OutputIds,
                 IsAsync=model.IsAsync,
                CorrelationFrom = model.CorrelationFrom,
                CorrelationId = model.CorrelationId,
                CreateDate = model.CreateDate,
                Remark = model.Remark,
                UpdateDate = model.UpdateDate, 
                Creater = IdentityContext.Get()?.UserId,
                Updater = IdentityContext.Get()?.UserId,
            };
        }
    }
}

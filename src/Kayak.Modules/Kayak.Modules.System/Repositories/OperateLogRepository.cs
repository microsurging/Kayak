﻿using Kayak.Core.Common.Context;
using Kayak.Core.Common.Extensions;
using Kayak.Core.Common.Intercepte.Model;
using Kayak.Core.Common.Intercepte.Queries;
using Kayak.Core.Common.Repsitories;
using Kayak.Core.Common.Repsitories.Implementation;
using Kayak.Core.Common.Response;
using Kayak.DataAccess.DeviceData;
using Kayak.DataAccess.DeviceData.Entities;
using Kayak.IModuleServices.System.Models;
using Kayak.IModuleServices.System.Queries;
using Surging.Core.CPlatform.Ioc;
using Surging.Core.CPlatform.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kayak.Modules.System.Repositories
{
    public class OperateLogRepository : BaseRepository, ISingleInstance
    { 
        private readonly IEFRepository<OperateLog> _repository;
        public OperateLogRepository(IEFRepository<OperateLog> repository)
        { 
            _repository = repository;
        }

        public async Task<bool> Add(OperateLogModel model)
        {
            using (var context = DataContext.Instance())
            {
                context.OperateLog.Add(ToEntity(model));
                return await context.SaveChangesAsync() > 0;
            }
        }

        public Task<bool> DeleteById(List<int> ids)
        {
            using (var context = DataContext.Instance())
            {
                var result = _repository.Instance(context).ModifyBy(new OperateLog { IsDeleted = true }, p => ids.Contains(p.Id), "IsDeleted") > 0;
                return Task.FromResult(result);
            }
        }

        public async Task<Page<OperateLogModel>> GetPageAsync(OperateLogQuery query)
        {
            using (var context = DataContext.Instance())
            {
                var result = new Page<OperateLogModel>();
                var entities = await _repository.Instance(context).GetPageList(query.Page, query.PageSize, x => x
                .WhereIF(query.Id.HasValue, e => e.Id == query.Id)
                .WhereIF(!query.Ids.IsNullOrEmpty(), e => query.Ids.Contains(e.Id))
                .WhereIF(!query.RoutePath.IsNullOrEmpty(), e => e.RoutePath == query.RoutePath)
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

        public OperateLogModel ToModel(OperateLog entity)
        {
            return entity == null ? default : new OperateLogModel
            {
                Id = entity.Id,
                CreateDate = entity.CreateDate,
                Arguments = entity.Arguments,
                Payload = entity.Payload,
                ReturnType = entity.ReturnType,
                ReturnValue = entity.ReturnValue,
                RoutePath = entity.RoutePath,
                ServiceId = entity.ServiceId,
                 RunTime = entity.RunTime,
            };
        }

        public OperateLog ToEntity(OperateLogModel model)
        {
            return model == null ? default : new OperateLog
            {
                CreateDate = model.CreateDate,
                Arguments = model.Arguments,
                Payload = model.Payload,
                ReturnType = model.ReturnType,
                ReturnValue = model.ReturnValue,
                RoutePath = model.RoutePath,
                ServiceId = model.ServiceId,
                 RunTime = model.RunTime,
                  Creater=IdentityContext.Get()?.UserId
            };
        }
    }
}

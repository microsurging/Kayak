using Kayak.Core.Common.Intercepte.Model;
using Kayak.Core.Common.Intercepte.Queries;
using Kayak.Core.Common.Repsitories;
using Kayak.Core.Common.Response;
using Kayak.DataAccess.DeviceData.Entities;
using Kayak.DataAccess.DeviceData;
using Surging.Core.CPlatform.Ioc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kayak.Core.Common.Extensions;
using Kayak.IModuleServices.System.Queries;
using Surging.Core.System.MongoProvider;
using Kayak.IModuleServices.System.Models;
using Microsoft.Extensions.Logging;
using Surging.Core.CPlatform.Network;
using Kayak.Core.Common.Context;

namespace Kayak.Modules.System.Repositories
{
    internal class NetworkLogRepository : BaseRepository, ISingleInstance
    { 
        private readonly IEFRepository<NetworkLog> _repository;
        public NetworkLogRepository(IEFRepository<NetworkLog> repository)
        { 
            _repository = repository;
        }

        public async Task<bool> Add(NetworkLogModel model)
        {
            using (var context = DataContext.Instance())
            {
                context.NetworkLog.Add(ToEntity(model));
                return await context.SaveChangesAsync() > 0;
            }
        }

        public Task<bool> DeleteById(List<int> ids)
        {
            using (var context = DataContext.Instance())
            {
                var result = _repository.Instance(context).ModifyBy(new NetworkLog { IsDeleted = true }, p => ids.Contains(p.Id), "IsDeleted") > 0;
                return Task.FromResult(result);
            }
        }

        public async Task<Page<NetworkLogModel>> GetPageAsync(NetworkLogQuery query)
        {
            using (var context = DataContext.Instance())
            {
                var result = new Page<NetworkLogModel>();
                var entities = await _repository.Instance(context).GetPageList(query.Page, query.PageSize, x => x
                .WhereIF(query.Id.HasValue, e => e.Id == query.Id)
                .WhereIF(!query.Ids.IsNullOrEmpty(), e => query.Ids.Contains(e.Id))
                .WhereIF(!query.NetworkId.IsNullOrEmpty(), e => e.NetworkId == query.NetworkId)
                .WhereIF(query.StartTime != null && query.EndTime != null, e => e.CreateDate >= query.StartTime && e.CreateDate <= query.EndTime)
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

        public NetworkLogModel ToModel(NetworkLog entity)
        {
            return entity == null ? default : new NetworkLogModel
            {
                Id = entity.Id,
                CreateDate = entity.CreateDate,
                EventName = entity.EventName,
                NetworkId = entity.NetworkId,
                  
                Content = entity.Content,
                logLevel = Enum.Parse<LogLevel>(entity.logLevel.ToString(), true),
                NetworkType = Enum.Parse <NetworkType>( entity.NetworkType.ToString(), true), 
                logLevelName = Enum.Parse<LogLevel>(entity.logLevel.ToString(), true).ToString(),
                NetworkTypeName = Enum.Parse<NetworkType>(entity.NetworkType.ToString(), true).ToString(),

            };
          
        }

        public NetworkLog ToEntity(NetworkLogModel model)
        {
            return model == null ? default : new NetworkLog
            {
                CreateDate = model.CreateDate,
                EventName = model.EventName,
                NetworkId = model.NetworkId,
                Content = model.Content,
                logLevel = (int)model.logLevel,
                NetworkType = (int)model.NetworkType,
                Updater = IdentityContext.Get()?.UserId,
                Creater = IdentityContext.Get()?.UserId,
            };
        }
    }
}


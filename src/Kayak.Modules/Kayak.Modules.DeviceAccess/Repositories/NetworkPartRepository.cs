using Kayak.Core.Common.Repsitories;
using Kayak.Core.Common.Response;
using Kayak.IModuleServices.DeviceAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Kayak.IModuleServices.DeviceAccess.Queries;
using Kayak.Core.Common.Extensions;
using Surging.Core.CPlatform.Ioc;
using Kayak.DataAccess.DeviceData;
using Kayak.DataAccess.DeviceData.Entities;
using Surging.Core.CPlatform.Utilities;
using Kayak.Core.Common.Context;

namespace Kayak.Modules.DeviceAccess.Repositories
{
    public class NetworkPartRepository:BaseRepository, ISingleInstance
    { 
        private readonly IEFRepository<NetworkPart> _repository;
        public NetworkPartRepository(IEFRepository<NetworkPart> repository)
        { 
            _repository = repository;
        }

        public async Task<bool> Add(NetworkPartModel model)
        {
            using (var context = DataContext.Instance())
            {
                context.NetworkPart.Add(ToEntity(model));
                return await context.SaveChangesAsync() > 0;
            }
        }
  
        public async Task<List<NetworkPartModel>> GetListByIds(List<int> ids)
        {
            using (var context = DataContext.Instance())
            {
                var entities = await context.NetworkPart.Where(p => ids.Contains(p.Id)).ToListAsync();
                return entities.Select(p => ToModel(p)).ToList();
            }
        }

        public async Task<Page<NetworkPartModel>> GetPageAsync(NetworkPartQuery query)
        {
            using (var context = DataContext.Instance())
            {
                var result = new Page<NetworkPartModel>();
                var entities = await _repository.Instance(context).GetPageList(query.Page, query.PageSize, x => x
                .WhereIF(query.Id.HasValue, e => e.Id == query.Id)
                .WhereIF(!query.Ids.IsNullOrEmpty(), e => query.Ids.Contains(e.Id))
                .WhereIF(!query.Name.IsNullOrEmpty(), e => e.Name.StartsWith(query.Name))
                .WhereIF(query.ComponentTypeCode != null, e => e.ComponentTypeCode == query.ComponentTypeCode)
                .WhereIF(query.Status != null, e => e.Status == query.Status)
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

        public async Task<bool> ExistsModelByName(string name)
        {
            using (var context = DataContext.Instance())
            {
                return await context.NetworkPart.AsNoTracking().AnyAsync(p => p.Name == name);
            }
        }

       public  async Task<NetworkPartModel> GetNetworkPartById(int id)
        {
            using (var context = DataContext.Instance())
            {
                var entity = await context.NetworkPart.Where(p => p.Id == id).FirstOrDefaultAsync();
                return ToModel(entity);
            }
        }

        public async Task<bool> DeleteById(List<int> ids)
        {
            using (var context = DataContext.Instance())
            {
                var result = _repository.Instance(context).ModifyBy(new NetworkPart { IsDeleted = true }, p => ids.Contains(p.Id), "IsDeleted") > 0;
                return await Task.FromResult(result);
            }
        }

        public async Task<bool> Stop(List<int> ids)
        {
            using (var context = DataContext.Instance())
            {
                var result = _repository.Instance(context).ModifyBy(new NetworkPart { Status = 0 }, p => ids.Contains(p.Id), "Status") > 0;
                return await Task.FromResult(result);
            }
        }

        public async Task<bool> Open(List<int> ids)
        {
            using (var context = DataContext.Instance())
            {
                var result = _repository.Instance(context).ModifyBy(new NetworkPart { Status = 1 }, p => ids.Contains(p.Id), "Status") > 0;
                return await Task.FromResult(result);
            }
        }

        public async Task<bool> Modify(NetworkPartModel model)
        {
            using (var context = DataContext.Instance())
            {
                var entity = ToEntity(model);
                var result = _repository.Instance(context).ModifyBy(entity, p => p.Id == model.Id, "Name", "ComponentTypeCode", "ClusterMode", "EnableSwagger", "ResolveMode", "EnableWebService", "IsMulticast", "Status", "EnableSSL", "RuleScript", "Host", "Port", "Remark", "Updater","UpdateDate") > 0;
                return await Task.FromResult(result);
            }
        }

        public async Task<List<NetworkPartModel>> GetNetworkPartByCondition(NetworkPartQuery query)
        {
            using (var context = DataContext.Instance())
            {
                var queryable = context.NetworkPart.AsNoTracking().Where(p => p.IsDeleted == false);
                if (query.Name != null)
                {
                    queryable = queryable.Where(p => p.Name.Contains(query.Name));
                }

                if (query.ComponentTypeCode != null)
                {
                    queryable = queryable.Where(p => p.ComponentTypeCode == query.ComponentTypeCode);
                }

                if (query.Status != null)
                {
                    queryable = queryable.Where(p => p.Status == query.Status);
                }

                var list = await queryable.ToListAsync();
                return list.Select(p => ToModel(p)).ToList();
            }
        }

        public NetworkPartModel ToModel(NetworkPart entity)
        {
            return entity == null ? default : new NetworkPartModel
            {
                Id = entity.Id,
                ClusterModeId = entity.ClusterModeId,
                Delimited = entity.Delimited,
                EnableTLS = entity.EnableTLS,
                FixedLength = entity.FixedLength,
                MaxMessageLength = entity.MaxMessageLength,
                ResolveMode = entity.ResolveMode,
                ComponentTypeCode = entity.ComponentTypeCode,
                CreateDate = entity.CreateDate,
                 IsMulticast = entity.IsMulticast,
                EnableSSL = entity.EnableSSL,
                EnableSwagger = entity.EnableSwagger,
                EnableWebService = entity.EnableWebService,
                Host = entity.Host,
                Name = entity.Name,
                Port = entity.Port,
                Remark = entity.Remark,
                RuleScript = entity.RuleScript,
                Status = entity.Status,
                UpdateDate = entity.UpdateDate,
            };
        }

        public NetworkPart ToEntity(NetworkPartModel model)
        {
            return model == null ? default : new NetworkPart
            {
                ClusterModeId = model.ClusterModeId,
                ComponentTypeCode = model.ComponentTypeCode,
                CreateDate = model.CreateDate,
                EnableSSL = model.EnableSSL,
                Delimited = model.Delimited,
                EnableSwagger = model.EnableSwagger,
                EnableWebService = model.EnableWebService,
                EnableTLS = model.EnableTLS,
                 IsMulticast=model.IsMulticast,
                FixedLength = model.FixedLength,
                MaxMessageLength = model.MaxMessageLength,
                ResolveMode = model.ResolveMode,
                Host = model.Host,
                Name = model.Name,
                Port = model.Port,
                Remark = model.Remark,
                RuleScript = model.RuleScript,
                Status = model.Status,
                UpdateDate = model.UpdateDate,
                Creater = IdentityContext.Get()?.UserId,
                Updater = IdentityContext.Get()?.UserId,
            };
        }
    }
}

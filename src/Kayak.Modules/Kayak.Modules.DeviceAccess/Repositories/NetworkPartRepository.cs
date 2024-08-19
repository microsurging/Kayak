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

namespace Kayak.Modules.DeviceAccess.Repositories
{
    public class NetworkPartRepository:BaseRepository, ISingleInstance
    {
        private readonly DataContext _dataContext;
        private readonly IEFRepository<NetworkPart> _repository;
        public NetworkPartRepository(IEFRepository<NetworkPart> repository)
        {
            _dataContext = ServiceLocator.GetService<DataContext>(AppConfig.DeviceDataOptions.DatabaseType.ToString());
            _repository = repository;
        }

        public async Task<bool> Add(NetworkPartModel model) 
        {
            _dataContext.NetworkPart.Add(ToEntity(model));
            return await _dataContext.SaveChangesAsync() > 0;
        }

        public async Task<NetworkPartModel> GetProtocol(int id)
        {
            var entity = await _dataContext.NetworkPart.Where(p => p.Id == id).FirstOrDefaultAsync();
            return ToModel(entity);
        }

        public async Task<Page<NetworkPartModel>> GetPageAsync(NetworkPartQuery query)
        {
            var result = new Page<NetworkPartModel>();
            var entities = await _repository.Instance(_dataContext).GetPageList(query.Page, query.PageSize, x => x
            .WhereIF(query.Id.HasValue, e => e.Id == query.Id)
            .WhereIF(!query.Ids.IsNullOrEmpty(), e => query.Ids.Contains(e.Id))
            .WhereIF(!query.Name.IsNullOrEmpty(), e => e.Name.StartsWith(query.Name))
            .WhereIF(!query.ComponentType.IsNullOrEmpty(), e => e.ComponentType==query.ComponentType)
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

        public NetworkPartModel ToModel(NetworkPart entity)
        {
            return entity == null ? default : new NetworkPartModel
            {
                ClusterMode = entity.ClusterMode,
                ComponentType = entity.ComponentType,
                CreateDate = entity.CreateDate,
                EnableSSL = entity.EnableSSL,
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
                ClusterMode = model.ClusterMode,
                ComponentType = model.ComponentType,
                CreateDate = model.CreateDate,
                EnableSSL = model.EnableSSL,
                Host = model.Host,
                Name = model.Name,
                Port = model.Port,
                Remark = model.Remark,
                RuleScript = model.RuleScript,
                Status = model.Status,
                UpdateDate = model.UpdateDate,

            };
        }
    }
}

using Kayak.Core.Common.Context;
using Kayak.Core.Common.Extensions;
using Kayak.Core.Common.Repsitories;
using Kayak.Core.Common.Repsitories.Implementation;
using Kayak.Core.Common.Response;
using Kayak.DataAccess.DeviceData;
using Kayak.DataAccess.DeviceData.Entities;
using Kayak.IModuleServices.DeviceAccess.Models;
using Kayak.IModuleServices.DeviceAccess.Queries;
using Microsoft.EntityFrameworkCore;
using Surging.Core.CPlatform.Ioc;
using Surging.Core.System.MongoProvider;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kayak.Modules.DeviceAccess.Repositories
{
    public class DeviceGatewayRepository : BaseRepository, ISingleInstance
    { 
        private readonly IEFRepository<DeviceGateway> _repository;
        public DeviceGatewayRepository(IEFRepository<DeviceGateway> repository)
        { 
            _repository = repository;
        }

        public async Task<bool> Add(DeviceGatewayModel model)
        {
            using (var context = DataContext.Instance())
            {
                context.DeviceGateway.Add(ToEntity(model));
                return await context.SaveChangesAsync() > 0;
            }
        }

       public async Task<DeviceGatewayModel> GetModelById(int gatewayId)
        {
            using (var context = DataContext.Instance())
            {
               var entity= await  (from q in context.DeviceGateway where q.Id == gatewayId select q).FirstOrDefaultAsync();
               return ToModel(entity);
            }
        }

        public async Task<Page<DeviceGatewayModel>> GetPageAsync(DeviceGatewayQuery query)
        {
            using (var context = DataContext.Instance())
            {
                var result = new Page<DeviceGatewayModel>();
                var entities = await _repository.Instance(context).GetPageList(query.Page, query.PageSize, x => x
                .WhereIF(query.Id.HasValue, e => e.Id == query.Id)
                .WhereIF(!query.Ids.IsNullOrEmpty(), e => query.Ids.Contains(e.Id))
                .WhereIF(!query.Name.IsNullOrEmpty(), e => e.Name.StartsWith(query.Name))
                .WhereIF(query.Status!=null, e => e.Status==query.Status)
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

        public async Task<bool> Modify(DeviceGatewayModel model)
        {
            using (var context = DataContext.Instance())
            {
                var entity = ToEntity(model);
                var result = _repository.Instance(context).ModifyBy(entity, p => p.Id == model.Id, "Name", "GatewayTypeValue", "NetWorkId", "ProtocolCode", "Status", "Remark", "Updater", "UpdateDate") > 0;
                return await Task.FromResult(result);
            }
        }

        public async Task<bool> Stop(List<int> ids)
        {
            using (var context = DataContext.Instance())
            {
                var result = _repository.Instance(context).ModifyBy(new DeviceGateway { Status = 0 }, p => ids.Contains(p.Id), "Status") > 0;
                return await Task.FromResult(result);
            }
        }

        public async Task<bool> Open(List<int> ids)
        {
            using (var context = DataContext.Instance())
            {
                var result = _repository.Instance(context).ModifyBy(new DeviceGateway { Status = 1 }, p => ids.Contains(p.Id), "Status") > 0;
                return await Task.FromResult(result);
            }
        }

        public async Task<bool> DeleteById(List<int> ids)
        {
            using (var context = DataContext.Instance())
            {
                var result = _repository.Instance(context).ModifyBy(new DeviceGateway { IsDeleted = true }, p => ids.Contains(p.Id), "IsDeleted") > 0;
                return await Task.FromResult(result);
            }
        }

        public DeviceGatewayModel ToModel(DeviceGateway entity)
        {
            return entity == null ? default : new DeviceGatewayModel
            {
                Id = entity.Id,
                GatewayTypeValue = entity.GatewayTypeValue,
                Name = entity.Name,
                NetWorkId = entity.NetWorkId,
                ProtocolCode = entity.ProtocolCode,
                Remark = entity.Remark,
                 CreateDate  = entity.CreateDate,
                  Status = entity.Status,
                   UpdateDate = entity.UpdateDate,
            };
        }

        public DeviceGateway ToEntity(DeviceGatewayModel model)
        {
            return model == null ? default : new DeviceGateway
            {
                GatewayTypeValue = model.GatewayTypeValue,
                Name = model.Name,
                NetWorkId = model.NetWorkId,
                ProtocolCode = model.ProtocolCode,
                 CreateDate=model.CreateDate,
                 Remark= model.Remark,
                 Status = model.Status,
                  UpdateDate = model.UpdateDate, 
                Creater = IdentityContext.Get()?.UserId,
                Updater = IdentityContext.Get()?.UserId,
            };
        }
    }
}

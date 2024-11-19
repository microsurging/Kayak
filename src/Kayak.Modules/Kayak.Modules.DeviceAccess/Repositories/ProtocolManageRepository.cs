using Kayak.Core.Common.Repsitories;
using Kayak.Core.Common.Response;
using Kayak.IModuleServices.DeviceAccess.Models;
using Kayak.IModuleServices.DeviceAccess.Queries;
using Kayak.Core.Common.Extensions;
using Microsoft.EntityFrameworkCore;
using Surging.Core.CPlatform.Ioc;
using Kayak.DataAccess.DeviceData;
using Kayak.DataAccess.DeviceData.Entities;
using Surging.Core.CPlatform.Utilities;
using Kayak.Core.Common.Context;

namespace Kayak.Modules.DeviceAccess.Repositories
{
    public class ProtocolManageRepository : BaseRepository, ISingleInstance
    { 
        private readonly IEFRepository<Protocol> _repository;
        public ProtocolManageRepository(IEFRepository<Protocol> repository)
        { 
            _repository = repository;
        }

        public async Task<bool> Add(ProtocolModel model)
        {
            using (var context = DataContext.Instance())
            {
                context.Protocol.Add(ToEntity(model));
                return await context.SaveChangesAsync() > 0;
            }
        }

        public Task<bool> Modify(ProtocolModel model)
        {
            using (var context = DataContext.Instance())
            {
                var entity = ToEntity(model);
                var result = _repository.Instance(context).ModifyBy(entity, p => p.Id == model.Id, "ProtocolName", "ComponentType", "ClassName", "UpdateDate", "Remark", "FileAddress", "ConnProtocol", "Script", "Updater") > 0;
                return Task.FromResult(result);
            }
        }

        public Task<bool> Republish(List<int> ids)
        {
            using (var context = DataContext.Instance())
            {
                var entity = new Protocol { Status = 1 };
                var result = _repository.Instance(context).ModifyBy(entity, p => ids.Contains(p.Id), "Status") > 0;
                return Task.FromResult(result);
            }
        }

        public Task<bool> CancelPublish(List<int> ids)
        {
            using (var context = DataContext.Instance())
            {
                var entity = new Protocol { Status = 0 };
                var result = _repository.Instance(context).ModifyBy(entity, p => ids.Contains(p.Id), "Status") > 0;
                return Task.FromResult(result);
            }
        }

        public Task<bool> DeleteById(List<int> ids)
        {
            using (var context = DataContext.Instance())
            {
                var result = _repository.Instance(context).ModifyBy(new Protocol { IsDeleted = true }, p => ids.Contains(p.Id), "IsDeleted") > 0;
                return Task.FromResult(result);
            }
        }
        public async Task<List<ProtocolModel>> GetProtocols()
        {
            using (var context = DataContext.Instance())
            {
                var list = await (from q in context.Protocol select q).ToListAsync();
                return list.Select(p => ToModel(p)).ToList();
            }
        }

        public async Task<ProtocolModel> GetProtocol(int id)
        {
            using (var context = DataContext.Instance())
            {
                var entity = await context.Protocol.Where(p => p.Id == id).FirstOrDefaultAsync();
                return ToModel(entity);
            }
        }

        public async Task<ProtocolModel> GetProtocolByCode(string code)
        {
            using (var context = DataContext.Instance())
            {
                var entity = await context.Protocol.Where(p => p.ProtocolCode == code).FirstOrDefaultAsync();
                return ToModel(entity);
            }
        }

        public async Task<Page<ProtocolModel>> GetPageAsync(ProtocolQuery query)
        {
            using (var context = DataContext.Instance())
            {
                var result = new Page<ProtocolModel>();
                var entities = await _repository.Instance(context).GetPageList(query.Page, query.PageSize, x => x
                .WhereIF(query.Id.HasValue, e => e.Id == query.Id)
                .WhereIF(!query.Ids.IsNullOrEmpty(), e => query.Ids.Contains(e.Id))
                .WhereIF(!query.ProtocolName.IsNullOrEmpty(), e => e.ProtocolName.StartsWith(query.ProtocolName))
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
         
        public ProtocolModel ToModel(Protocol entity)
        {
            return entity == null ? default : new ProtocolModel
            {
                Id = entity.Id,
                ClassName = entity.ClassName,
                 ProtocolType = entity.ProtocolType,
                CreateDate = entity.CreateDate,
                FileAddress = entity.FileAddress,
                ProtocolCode = entity.ProtocolCode,
                Status = entity.Status,
                 Script = entity.Script,
                ConnProtocol = entity.ConnProtocol,
                Remark = entity.Remark,
                ProtocolName = entity.ProtocolName,
                UpdateDate = entity.UpdateDate,

            };
        }

        public Protocol ToEntity(ProtocolModel model)
        {
            return model == null ? default : new Protocol
            {
                ClassName = model.ClassName,
                 Script = model.Script,
                ProtocolType = model.ProtocolType,
                CreateDate = model.CreateDate,
                FileAddress = model.FileAddress,
                ProtocolCode = model.ProtocolCode,
                Status = model.Status,
                 ConnProtocol=model.ConnProtocol,
                Remark = model.Remark,
                ProtocolName = model.ProtocolName,
                UpdateDate = model.UpdateDate, 
                Updater = IdentityContext.Get()?.UserId,
                Creater = IdentityContext.Get()?.UserId,
            };
        }
    }
}

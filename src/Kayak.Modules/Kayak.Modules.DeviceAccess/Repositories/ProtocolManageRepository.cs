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

namespace Kayak.Modules.DeviceAccess.Repositories
{
    public class ProtocolManageRepository : BaseRepository, ISingleInstance
    {
        private readonly DataContext _dataContext;
        private readonly IEFRepository<Protocol> _repository;
        public ProtocolManageRepository(IEFRepository<Protocol> repository) {
            _dataContext = ServiceLocator.GetService<DataContext>(AppConfig.DeviceDataOptions.DatabaseType.ToString());
            _repository = repository;
        }

        public async Task<bool> Add(ProtocolModel model)
        {
            _dataContext.Protocol.Add(ToEntity(model));
            return await _dataContext.SaveChangesAsync() > 0;
        }

       public Task<bool> Republish(List<int> ids)
        {
            var entity=new Protocol {   Status =1};
            var result= _repository.Instance(_dataContext).ModifyBy(entity, p => ids.Contains(p.Id), "Status")>0;
            return Task.FromResult(result);
        }

        public Task<bool> CancelPublish(List<int> ids)
        {
            var entity = new Protocol {Status = 0 };
            var result = _repository.Instance(_dataContext).ModifyBy(entity, p => ids.Contains(p.Id), "Status") > 0;
            return Task.FromResult(result);
        }

       public   Task<bool> DeleteById(List<int> ids)
        {
            var result = _repository.Instance(_dataContext).ModifyBy(new Protocol { IsDeleted = true }, p => ids.Contains(p.Id), "IsDeleted") > 0;
            return Task.FromResult(result);
        }

        public async  Task<ProtocolModel> GetProtocol(int id)
        {
            var entity = await _dataContext.Protocol.Where(p => p.Id == id).FirstOrDefaultAsync();
            return ToModel(entity);
        }

        public async Task<Page<ProtocolModel>> GetPageAsync(ProtocolQuery query)
        {
            var result = new Page<ProtocolModel>();
            var entities = await _repository.Instance(_dataContext).GetPageList(query.Page, query.PageSize, x => x
            .WhereIF(query.Id.HasValue, e => e.Id == query.Id)
            .WhereIF(!query.Ids.IsNullOrEmpty(), e => query.Ids.Contains(e.Id))
            .WhereIF(!query.ProtocolName.IsNullOrEmpty(), e => e.ProtocolName.StartsWith(query.ProtocolName)) 
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


        public ProtocolModel ToModel(Protocol entity)
        {
            return entity == null ? default : new ProtocolModel
            {
                  Id = entity.Id,
                ClassName = entity.ClassName,
                ComponentType = entity.ComponentType,
                CreateDate = entity.CreateDate,
                FileAddress = entity.FileAddress,
                ProtocolCode = entity.ProtocolCode,
                 Status = entity.Status,
                 Remark=entity.Remark,
                ProtocolName = entity.ProtocolName,
                UpdateDate = entity.UpdateDate,

            };
        }

        public Protocol ToEntity(ProtocolModel model)
        {
            return model == null ? default : new Protocol
            {
                ClassName = model.ClassName,
                ComponentType = model.ComponentType,
                CreateDate = model.CreateDate,
                FileAddress = model.FileAddress,
                ProtocolCode = model.ProtocolCode,
                 Status=model.Status,
                  Remark=model.Remark,
                ProtocolName = model.ProtocolName,
                UpdateDate = model.UpdateDate,

            };
        }
    }
}

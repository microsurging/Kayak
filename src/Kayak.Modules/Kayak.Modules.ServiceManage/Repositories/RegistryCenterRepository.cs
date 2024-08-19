using Kayak.Core.Common.Repsitories;
using Kayak.Core.Common.Response;
using Kayak.DataAccess.DeviceData;
using Kayak.DataAccess.DeviceData.Entities;
using Kayak.IModuleServices.ServiceManage.Model;
using Kayak.IModuleServices.ServiceManage.Query;
using Surging.Core.CPlatform.Ioc;
using Kayak.Core.Common.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Kayak.Modules.ServiceManage.Repositories
{
    public class RegistryCenterRepository : BaseRepository, ISingleInstance
    {
        private readonly IEFRepository<RegistryCenter> _repository;
        private readonly DataContext _dataContext;
        public RegistryCenterRepository(IEFRepository<RegistryCenter> repository)
        {
            _repository = repository;
            _dataContext = DataContext.Instance();
        }

        public async Task<bool> Add(RegistryCenterModel model)
        {
            _dataContext.RegistryCenter.Add(ToEntity(model));
            return await _dataContext.SaveChangesAsync() > 0;
        }

        public async Task<List<RegistryCenterModel>> GetList()
        {
            var list = await _dataContext.RegistryCenter.Where(p => p.IsDeleted == false).ToListAsync();
            return list.Select(p => ToModel(p)).ToList();
        }

        public Task<bool> DeleteById(List<int> ids)
        {
            var result = _repository.Instance(_dataContext).ModifyBy(new RegistryCenter { IsDeleted = true }, p => ids.Contains(p.Id), "IsDeleted") > 0;
            return Task.FromResult(result);
        }

        public async Task<Page<RegistryCenterModel>> GetPageAsync(RegistryCenterQuery query)
        {
            var result = new Page<RegistryCenterModel>();
            var entities = await _repository.Instance(_dataContext).GetPageList(query.Page, query.PageSize, x => x
            .WhereIF(query.Id.HasValue, e => e.Id == query.Id)
            .WhereIF(!query.Ids.IsNullOrEmpty(), e => query.Ids.Contains(e.Id))
            .WhereIF(!query.Name.IsNullOrEmpty(), e => e.Name.Contains(query.Name))
            .WhereIF(query.RegistryCenterType!=null, e => e.RegistryCenterType==query.RegistryCenterType)
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

        public async Task<bool> Modify(RegistryCenterModel model)
        {
            var entity = ToEntity(model);
            var result = _repository.Instance(_dataContext).ModifyBy(entity, p => p.Id == model.Id, "Path", "Name", "Host", "Port", "Remark", "RegistryCenterType", "UpdateDate") > 0;
            return await Task.FromResult(result);
        }

        private RegistryCenter ToEntity(RegistryCenterModel model)
        {

            return model == null ? default : new RegistryCenter
            {
                 Path = model.Path,
                Name = model.Name,
                CreateDate = model.CreateDate,
                 Host = model.Host,
                RegistryCenterType = model.RegistryCenterType,
                Remark = model.Remark,
                 Port = model.Port, 
            };
        }


        private RegistryCenterModel ToModel(RegistryCenter entity)
        {

            return entity == null ? default : new RegistryCenterModel
            {
                 Id = entity.Id,
                Path = entity.Path,
                Name = entity.Name,
                CreateDate = entity.CreateDate,
                Host = entity.Host,
                RegistryCenterType = entity.RegistryCenterType,
                Remark = entity.Remark,
                Port = entity.Port,

            };
        }
    }
}

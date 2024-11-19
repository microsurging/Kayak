using Kayak.Core.Common.Repsitories;
using Kayak.Core.Common.Response;
using Kayak.DataAccess.DeviceData;
using Kayak.DataAccess.DeviceData.Entities;
using Kayak.IModuleServices.ServiceManage.Model;
using Kayak.IModuleServices.ServiceManage.Query;
using Surging.Core.CPlatform.Ioc;
using Kayak.Core.Common.Extensions;
using Microsoft.EntityFrameworkCore;
using Kayak.Core.Common.Context;

namespace Kayak.Modules.ServiceManage.Repositories
{
    public class RegistryCenterRepository : BaseRepository, ISingleInstance
    {
        private readonly IEFRepository<RegistryCenter> _repository;
        public RegistryCenterRepository(IEFRepository<RegistryCenter> repository)
        {
            _repository = repository;
        }

        public async Task<bool> Add(RegistryCenterModel model)
        {
            using (var context = DataContext.Instance())
            {
                context.RegistryCenter.Add(ToEntity(model));
                return await context.SaveChangesAsync() > 0;
            }
        }

        public async Task<List<RegistryCenterModel>> GetList()
        {
            using (var context = DataContext.Instance())
            {
                var list = await context.RegistryCenter.Where(p => p.IsDeleted == false).ToListAsync();
                return list.Select(p => ToModel(p)).ToList();
            }
        }

        public Task<bool> DeleteById(List<int> ids)
        {
            using (var context = DataContext.Instance())
            {
                var result = _repository.Instance(context).ModifyBy(new RegistryCenter { IsDeleted = true }, p => ids.Contains(p.Id), "IsDeleted") > 0;
                return Task.FromResult(result);
            }
        }

        public async Task<Page<RegistryCenterModel>> GetPageAsync(RegistryCenterQuery query)
        {
            using (var context = DataContext.Instance())
            {
                var result = new Page<RegistryCenterModel>();
                var entities = await _repository.Instance(context).GetPageList(query.Page, query.PageSize, x => x
                .WhereIF(query.Id.HasValue, e => e.Id == query.Id)
                .WhereIF(!query.Ids.IsNullOrEmpty(), e => query.Ids.Contains(e.Id))
                .WhereIF(!query.Name.IsNullOrEmpty(), e => e.Name.Contains(query.Name))
                .WhereIF(query.RegistryCenterType != null, e => e.RegistryCenterType == query.RegistryCenterType)
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

        public async Task<bool> Modify(RegistryCenterModel model)
        {
            using (var context = DataContext.Instance())
            {
                var entity = ToEntity(model);
                var result = _repository.Instance(context).ModifyBy(entity, p => p.Id == model.Id, "Path", "Name", "Host", "Port", "Remark", "RegistryCenterType", "Updater", "UpdateDate") > 0;
                return await Task.FromResult(result);
            }
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
                Updater = IdentityContext.Get()?.UserId,
                Creater = IdentityContext.Get()?.UserId,
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

using Kayak.Core.Common.Repsitories;
using Kayak.Core.Common.Response;
using Kayak.DataAccess.DeviceData.Entities;
using Kayak.DataAccess.DeviceData;
using Kayak.IModuleServices.ServiceManage.Model;
using Kayak.IModuleServices.ServiceManage.Query;
using Surging.Core.CPlatform.Ioc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Kayak.Core.Common.Extensions;
using Kayak.Core.Common.Context;

namespace Kayak.Modules.ServiceManage.Repositories
{
    public class ModuleRepository : BaseRepository, ISingleInstance
    {
        private readonly IEFRepository<Module> _repository;
        public ModuleRepository(IEFRepository<Module> repository)
        {
            _repository = repository;
        }

        public async Task<bool> Add(ModuleModel model)
        {
            using (var context = DataContext.Instance())
            {
                context.Module.Add(ToEntity(model));
                return await context.SaveChangesAsync() > 0;
            }
        }

        public async Task<List<ModuleModel>> GetList()
        {
            using (var context = DataContext.Instance())
            {
                var list = await context.Module.Where(p => p.IsDeleted == false).ToListAsync();
                return list.Select(p => ToModel(p)).ToList();
            }
        }

        public Task<bool> DeleteById(List<int> ids)
        {
            using (var context = DataContext.Instance())
            {
                var result = _repository.Instance(context).ModifyBy(new Module { IsDeleted = true }, p => ids.Contains(p.Id), "IsDeleted") > 0;
                return Task.FromResult(result);
            }
        }

        public async Task<Page<ModuleModel>> GetPageAsync(ModuleQuery query)
        {
            using (var context = DataContext.Instance())
            {
                var result = new Page<ModuleModel>();
                var entities = await _repository.Instance(context).GetPageList(query.Page, query.PageSize, x => x
                .WhereIF(query.Id.HasValue, e => e.Id == query.Id)
                .WhereIF(!query.Ids.IsNullOrEmpty(), e => query.Ids.Contains(e.Id))
                .WhereIF(!query.ModuleCode.IsNullOrEmpty(), e => e.ModuleCode.Contains(query.ModuleCode))
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

        public async Task<bool> Modify(ModuleModel model)
        {
            using (var context = DataContext.Instance())
            {
                var entity = ToEntity(model);
                var result = _repository.Instance(context).ModifyBy(entity, p => p.Id == model.Id, "Path", "Name", "Host", "Port", "Remark", "RegistryCenterType", "Updater", "UpdateDate") > 0;
                return await Task.FromResult(result);
            }
        }

        private Module ToEntity(ModuleModel model)
        {

            return model == null ? default : new Module
            {
                CreateDate = DateTime.Now,
                ModuleCode = model.ModuleCode,
                FileAddress = model.FileAddress,
                ModuleName = model.ModuleName,
                ModuleType = model.ModuleType,
                 ModuleMode = model.ModuleMode,
                Remark = model.Remark,
                UpdateDate = DateTime.Now,
                Creater = IdentityContext.Get()?.UserId,
                Updater = IdentityContext.Get()?.UserId
            };
        }


        private ModuleModel ToModel(Module entity)
        {

            return entity == null ? default : new ModuleModel
            {
                CreateDate = DateTime.Now,
                ModuleCode = entity.ModuleCode,
                FileAddress = entity.FileAddress,
                ModuleName = entity.ModuleName,
                 ModuleMode =entity.ModuleMode,
                ModuleType = entity.ModuleType,
                Remark = entity.Remark,
                UpdateDate = entity.UpdateDate,
                Id = entity.Id

            };
        }
    }
}
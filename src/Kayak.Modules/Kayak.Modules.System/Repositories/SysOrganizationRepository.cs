using Kayak.Core.Common.Repsitories;
using Kayak.Core.Common.Response;
using Kayak.IModuleServices.System.Models;
using Kayak.IModuleServices.System.Queries;
using Kayak.Core.Common.Enums;
using Microsoft.EntityFrameworkCore;
using Kayak.Core.Common.Extensions;
using Surging.Core.CPlatform.Ioc;
using Kayak.DataAccess.DeviceData;
using Kayak.DataAccess.DeviceData.Entities;
using Surging.Core.CPlatform.Utilities;

namespace Kayak.Modules.System.Repositories
{
    public class SysOrganizationRepository : BaseRepository, ISingleInstance
    {
        private readonly DataContext _dataContext;
        private readonly IEFRepository<SysOrganization> _repository;
        public SysOrganizationRepository(IEFRepository<SysOrganization> repository)
        {
            _dataContext = ServiceLocator.GetService<DataContext>(AppConfig.DeviceDataOptions.DatabaseType.ToString());
            _repository = repository;
        }

        public async Task<bool> Add(SysOrganizationModel model)
        {  
           var id=await _dataContext.SysOrganization.MaxAsync(p => p.Id);
            model.LevelCode = model.LevelCode.IsNullOrEmpty() ? (++id).ToString() : $"{++id}-{ model.LevelCode}";
            model.Level = model.LevelCode.Split("-").Length;
            await _dataContext.SysOrganization.AddAsync(ToEntity(model));
           
            return await _dataContext.SaveChangesAsync() > 0;
        }

        public async Task<bool> ExistsModelByName(string name)
        {
            return await _dataContext.SysOrganization.AsNoTracking().AnyAsync(p => p.Name == name);
        }

        public Task<bool> DeleteById(List<int> ids)
        {
            var result = _repository.Instance(_dataContext).ModifyBy(new SysOrganization { IsDeleted = true }, p => ids.Contains(p.Id), "IsDeleted") > 0;
            return Task.FromResult(result);
        }

        public async Task<List<SysOrganizationModel>> GetSysOrganizationByCondition(SysOrganizationQuery query)
        {
            var queryable = _dataContext.SysOrganization.AsNoTracking().Where(p => 1 == 1);
            if (query.LevelCode != null)
            {
                queryable = queryable.Where(p => p.LevelCode.EndsWith(query.LevelCode));
            }

            if (query.SysOrgType != null)
            {
                queryable = queryable.Where(p => p.SysOrgType == query.SysOrgType);
            }
            if(query.Level !=null)
            {
                queryable = queryable.Where(p => p.Level == query.Level);
            }

            if (query.Name != null)
            {
                queryable = queryable.Where(p => p.Name.Contains(query.Name));
            }
            var list = await queryable.ToListAsync();
            return list.Select(p => ToModel(p)).ToList();
        }

        public Task<bool> Modify(SysOrganizationModel model)
        {
            var entity = ToEntity(model);
            var result = _repository.Instance(_dataContext).ModifyBy(entity, p => p.Id == model.Id, "Address", "City", "Contacter", "Email", "Name", "Phone", "SysOrgType", "Remark", "UpdateDate") > 0;
            return Task.FromResult(result);
        }

        public async Task<Page<SysOrganizationModel>> GetPageAsync(SysOrganizationQuery query)
        {
            if(!query.LevelCode.IsNullOrEmpty() && !query.LevelCode.StartsWith("-"))
                query.LevelCode = "-" + query.LevelCode;
            var result = new Page<SysOrganizationModel>();
            var entities = await _repository.Instance(_dataContext).GetPageList(query.Page, query.PageSize, x => x
            .WhereIF(query.Id.HasValue, e => e.Id == query.Id)
            .WhereIF(!query.Ids.IsNullOrEmpty(), e => query.Ids.Contains(e.Id))
            .WhereIF(!query.LevelCode.IsNullOrEmpty(), e =>e.LevelCode.EndsWith(query.LevelCode ))
            .WhereIF(query.Level!=null, e => e.Level==query.Level)
            .WhereIF(query.SysOrgType != null, e => e.SysOrgType == query.SysOrgType)
             .WhereIF(!query.Name.IsNullOrEmpty(), e => e.Name.StartsWith(query.Name))
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

        public async Task<SysOrganizationModel> GetSysOrganization(int id)
        {
            var entity = await _dataContext.SysOrganization.Where(p => p.Id == id).FirstOrDefaultAsync();
            return ToModel(entity);
        }

        public SysOrganization ToEntity(SysOrganizationModel model)
        {
            return model == null ? default : new SysOrganization
            {
                Address = model.Address,
                City = model.City,
                Contacter = model.Contacter,
                CreateDate = model.CreateDate,
                Email = model.Email,
                LevelCode = model.LevelCode,
                Level = model.Level,
                Name = model.Name,
                Phone = model.Phone,
                SysOrgType = model.SysOrgType,
                UpdateDate = model.UpdateDate,
                Remark = model.Remark,

            };
        }

        public SysOrganizationModel ToModel(SysOrganization entity)
        {
            return entity == null ? default : new SysOrganizationModel
            {
                 Id = entity.Id,
                Address = entity.Address,
                City = entity.City,
                Contacter = entity.Contacter,
                CreateDate = entity.CreateDate,
                Email = entity.Email,
                LevelCode = entity.LevelCode,
                Level = entity.Level,
                Name = entity.Name,
                Phone = entity.Phone,
                SysOrgType =entity.SysOrgType,
                UpdateDate = entity.UpdateDate,
                Remark = entity.Remark,

            };
        }
    }
}

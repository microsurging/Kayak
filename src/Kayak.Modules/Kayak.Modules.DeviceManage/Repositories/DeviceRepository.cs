using Kayak.Core.Common.Repsitories;
using Kayak.DataAccess.DeviceData.Entities;
using Kayak.DataAccess.DeviceData;
using Surging.Core.CPlatform.Ioc;
using Surging.Core.CPlatform.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kayak.IModuleServices.DeviceManage.Models;
using Kayak.Core.Common.Response;
using Kayak.IModuleServices.DeviceManage.Queries;
using Kayak.Core.Common.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Kayak.Modules.DeviceManage.Repositories
{
    public  class DeviceRepository : BaseRepository, ISingleInstance
    {
        private readonly DataContext _dataContext;
        private readonly IEFRepository<Device> _repository;
        public DeviceRepository(IEFRepository<Device> repository)
        {
            _dataContext = DataContext.Instance();
            _repository = repository;
        }

        public async Task<bool> Add(DeviceModel model)
        {
            _dataContext.Device.Add(ToEntity(model));
            return await _dataContext.SaveChangesAsync() > 0;

        }

        public async Task<Page<DeviceModel>> GetPageAsync(DeviceQuery query)
        {
            var result = new Page<DeviceModel>();
            var entities = await _repository.Instance(_dataContext).GetPageList(query.Page, query.PageSize, x => x
            .WhereIF(query.Id.HasValue, e => e.Id == query.Id)
            .WhereIF(!query.Ids.IsNullOrEmpty(), e => query.Ids.Contains(e.Id))
            .WhereIF(query.ProductCodes != null, e => query.ProductCodes.Contains(e.ProductCode))
            .WhereIF(!query.Code.IsNullOrEmpty(), e => e.Code == query.Code)
             .WhereIF(!query.Name.IsNullOrEmpty(), e => e.Name == query.Name)
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

        public async Task<List<DeviceModel>> GetDeviceByIds(List<int> ids)
        {
            var list = await _dataContext.Device.AsNoTracking().Where(p => ids.Contains(p.Id)).ToListAsync();
            return list.Select(p => ToModel(p)).ToList();
        }

        public async Task<List<DeviceModel>> GetDeviceByCondition(DeviceQuery query)
        {
            var queryable = _dataContext.Device.AsNoTracking().Where(p => 1 == 1);
            if (query.Code != null)
            {
                queryable = queryable.Where(p => p.Code.Contains(query.Code));
            }

            if (query.ProductCodes != null)
            {
                queryable = queryable.Where(p => query.ProductCodes.Contains(p.ProductCode));
            }

            if (!query.Name.IsNullOrEmpty())
            {
                queryable = queryable.Where(p => p.Name==query.Name);
            }
            var list = await queryable.ToListAsync();
            return list.Select(p => ToModel(p)).ToList();
        }

        public Task<bool> DeleteById(List<int> ids)
        {
            var result = _repository.Instance(_dataContext).ModifyBy(new Device { IsDeleted = true }, p => ids.Contains(p.Id), "IsDeleted") > 0;
            return Task.FromResult(result);
        }

        public Task<bool> ChangeDisable(List<int> ids)
        {
            var result = _repository.Instance(_dataContext).ModifyBy(new Device { Status = 0 }, p => ids.Contains(p.Id), "Status") > 0;
            return Task.FromResult(result);
        }

        public Task<bool> ChangeEnable(List<int> ids)
        {
            var result = _repository.Instance(_dataContext).ModifyBy(new Device { Status = 1 }, p => ids.Contains(p.Id), "Status") > 0;
            return Task.FromResult(result);
        }

        public async Task<bool> ExistsModelByCode(string code)
        {
            return await _dataContext.ProductCategory.AsNoTracking().AnyAsync(p => p.Code == code);
        }

        public async Task<bool> Modify(DeviceModel model)
        {
            var entity = ToEntity(model);
            var result = _repository.Instance(_dataContext).ModifyBy(entity, p => p.Id == model.Id, "Code", "Name", "ProductCode", "Remark", "UpdateDate") > 0;
            return await Task.FromResult(result);
        }

        public Device ToEntity(DeviceModel model)
        {

            return model == null ? default : new Device
            {
                 Code = model.Code,
                  Name = model.Name,
                CreateDate = model.CreateDate,
                ProductCode = model.ProductCode,
                UpdateDate = model.UpdateDate,
                Remark = model.Remark
                 
            };
        }


        public DeviceModel ToModel(Device entity)
        {

            return entity == null ? default : new DeviceModel
            { 
                Id = entity.Id,
                Code = entity.Code,
                Name = entity.Name,
                CreateDate = entity.CreateDate,
                ProductCode = entity.ProductCode,
                UpdateDate = entity.UpdateDate,
                Remark = entity.Remark,
                Status = entity.Status,
                IsDeleted = entity.IsDeleted,

            };
        }

    }
}

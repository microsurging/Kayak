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
using Kayak.Core.Common.Context;

namespace Kayak.Modules.DeviceManage.Repositories
{
    public  class DeviceRepository : BaseRepository, ISingleInstance
    { 
        private readonly IEFRepository<Device> _repository;
        public DeviceRepository(IEFRepository<Device> repository)
        { 
            _repository = repository;
        }

        public async Task<bool> Add(DeviceModel model)
        {
            using (var context = DataContext.Instance())
            {
                context.Device.Add(ToEntity(model));
                return await context.SaveChangesAsync() > 0;
            } 
        }

        public async Task<Page<DeviceModel>> GetPageAsync(DeviceQuery query)
        {
            using (var context = DataContext.Instance())
            {
                var result = new Page<DeviceModel>();
                var entities = await _repository.Instance(context).GetPageList(query.Page, query.PageSize, x => x
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
        }

        public async Task<List<DeviceModel>> GetDeviceByIds(List<int> ids)
        {
            using (var context = DataContext.Instance())
            {
                var list = await context.Device.AsNoTracking().Where(p => ids.Contains(p.Id)).ToListAsync();
                return list.Select(p => ToModel(p)).ToList();
            }
        }

        public async Task<List<DeviceModel>> GetDeviceByCondition(DeviceQuery query)
        {
            using (var context = DataContext.Instance())
            {
                var queryable = context.Device.AsNoTracking().Where(p => 1 == 1);
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
                    queryable = queryable.Where(p => p.Name == query.Name);
                }
                var list = await queryable.ToListAsync();
                return list.Select(p => ToModel(p)).ToList();
            }
        }

        public Task<bool> DeleteById(List<int> ids)
        {
            using (var context = DataContext.Instance())
            {
                var result = _repository.Instance(context).ModifyBy(new Device { IsDeleted = true }, p => ids.Contains(p.Id), "IsDeleted") > 0;
                return Task.FromResult(result);
            }
        }

        public  async Task<List<DeviceStatistics>> GetDeviceCountByProductCodes(List<string> productCodes)
        {
            using (var context = DataContext.Instance())
            {
                var result = from p in context.Device.AsNoTracking()
                             group p by p.ProductCode into g
                             select new DeviceStatistics
                             {
                                 ProuductCode = g.Key,
                                 DeviceCount = g.Count()
                             };

                return await result.ToListAsync();
            }
        }

       public async Task<DeviceTotalStatisticsModel> GetDeviceTotalStatistics()
        {
            using (var context = DataContext.Instance())
            {
                var model = new DeviceTotalStatisticsModel();
                model.NormalCount = await (from q in context.Device where q.Status == 1 select q.Id)
                              .CountAsync();
                model.DisableCount = await (from q in context.Device where q.Status == 0 select q.Id)
                            .CountAsync();
                return model;
            }
        }
        public Task<bool> ChangeDisable(List<int> ids)
        {
            using (var context = DataContext.Instance())
            {
                var result = _repository.Instance(context).ModifyBy(new Device { Status = 0 }, p => ids.Contains(p.Id), "Status") > 0;
                return Task.FromResult(result);
            }
        }

        public Task<bool> ChangeEnable(List<int> ids)
        {
            using (var context = DataContext.Instance())
            {
                var result = _repository.Instance(context).ModifyBy(new Device { Status = 1 }, p => ids.Contains(p.Id), "Status") > 0;
                return Task.FromResult(result);
            }
        }

        public async Task<bool> ExistsModelByCode(string code)
        {
            using (var context = DataContext.Instance())
            {
                return await context.ProductCategory.AsNoTracking().AnyAsync(p => p.Code == code);
            }
        }

        public async Task<bool> Modify(DeviceModel model)
        {
            using (var context = DataContext.Instance())
            {
                var entity = ToEntity(model);
                var result = _repository.Instance(context).ModifyBy(entity, p => p.Id == model.Id, "Code", "Name", "ProductCode", "Remark", "Updater", "UpdateDate") > 0;
                return await Task.FromResult(result);
            }
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
                Remark = model.Remark,  
                Creater = IdentityContext.Get()?.UserId,
                Updater = IdentityContext.Get()?.UserId,

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

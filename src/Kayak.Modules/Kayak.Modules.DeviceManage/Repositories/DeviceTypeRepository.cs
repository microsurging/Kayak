using Kayak.Core.Common.Repsitories;
using Kayak.Core.Common.Response;
using Kayak.DataAccess.DeviceData.Entities;
using Kayak.DataAccess.DeviceData;
using Kayak.IModuleServices.DeviceManage.Models;
using Kayak.IModuleServices.DeviceManage.Queries;
using Surging.Core.CPlatform.Ioc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Surging.Core.System.MongoProvider;
using Kayak.Core.Common.Extensions;
using Kayak.Core.Common.Context;

namespace Kayak.Modules.DeviceManage.Repositories
{
    public class DeviceTypeRepository : BaseRepository, ISingleInstance
    {
        private readonly IEFRepository<DeviceType> _repository; 
        public DeviceTypeRepository(IEFRepository<DeviceType> repository)
        {
            _repository = repository; 
        }

        public async Task<bool> Add(DeviceTypeModel model)
        {
            using (var context = DataContext.Instance())
            {
                context.DeviceType.Add(ToEntity(model));
                return await context.SaveChangesAsync() > 0;
            }
        }

        public async Task<Page<DeviceTypeModel>> GetPageAsync(DeviceTypeQuery query)
        {
            using (var context = DataContext.Instance())
            {
                var result = new Page<DeviceTypeModel>();
                var entities = await _repository.Instance(context).GetPageList(query.Page, query.PageSize, x => x
                .WhereIF(query.Id.HasValue, e => e.Id == query.Id)
                .WhereIF(!query.Ids.IsNullOrEmpty(), e => query.Ids.Contains(e.Id))
                .WhereIF(query.DeviceTypeCode != null, e => e.DeviceTypeCode.Contains(query.DeviceTypeCode))
                .WhereIF(query.ProtocolCode != null, e => e.ProtocolCode == query.ProtocolCode)
                .WhereIF(query.OrganizationId != null, e => e.OrganizationId == query.OrganizationId)
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

        public async Task<bool> ExistsModelByCode(string code)
        {
            using (var context = DataContext.Instance())
            {
                return await context.DeviceType.AsNoTracking().AnyAsync(p => p.DeviceTypeCode == code);
            }
        }

        public async Task<bool> DeleteById(List<int> ids)
        {
            using (var context = DataContext.Instance())
            {
                var result = _repository.Instance(context).ModifyBy(new DeviceType { IsDeleted = true }, p => ids.Contains(p.Id), "IsDeleted") > 0;
                return await Task.FromResult(result);
            }
        }
 

        public async Task<bool> Modify(DeviceTypeModel model)
        {
            using (var context = DataContext.Instance())
            {
                var entity = ToEntity(model);
                var result = _repository.Instance(context).ModifyBy(entity, p => p.Id == model.Id, "ProductCode", "CategoryId", "DeviceType", "Protocol", "OrganizationId", "Remark", "Updater", "UpdateDate") > 0;
                return await Task.FromResult(result);
            }
        }

        public async Task<List<DeviceTypeModel>> GetDeviceTypeByCondition(DeviceTypeQuery query)
        {
            using (var context = DataContext.Instance())
            {
                var queryable = context.DeviceType.AsNoTracking().Where(p => 1 == 1);
                if (query.DeviceTypeCode != null)
                {
                    queryable = queryable.Where(p => p.DeviceTypeCode.Contains(query.DeviceTypeCode));
                }

                if (query.ProtocolCode != null)
                {
                    queryable = queryable.Where(p => p.ProtocolCode == query.ProtocolCode);
                }

                if (query.OrganizationId != null)
                {
                    queryable = queryable.Where(p => p.OrganizationId == query.OrganizationId);
                }

                var list = await queryable.ToListAsync();
                return list.Select(p => ToModel(p)).ToList();
            }
        }

        public async Task<List<DeviceTypeModel>> GetDeviceTypes()
        {
            using (var context = DataContext.Instance())
            {
                var list = await context.DeviceType.Where(p => p.IsDeleted == false).ToListAsync();
                return list.Select(p => ToModel(p)).ToList();
            }
        }

        public async Task<DeviceTypeModel> GetDeviceType(int id)
        {
            using (var context = DataContext.Instance())
            {
                var entity = await context.DeviceType.Where(p => p.Id == id).FirstOrDefaultAsync();
                return ToModel(entity);
            }
        }

        public DeviceTypeModel ToModel(DeviceType entity)
        {

            return entity == null ? default : new DeviceTypeModel
            {
                Id = entity.Id,
                ConnProtocolCode = entity.ConnProtocolCode,
                CreateDate = entity.CreateDate,
                DeviceTypeCode = entity.DeviceTypeCode,
                 Code=entity.Code,
                 DeviceTypeName = entity.DeviceTypeName , 
                ProductCategoryId = entity.ProductCategoryId,
                ProtocolCode = entity.ProtocolCode,
                OrganizationId = entity.OrganizationId,
                UpdateDate = entity.UpdateDate,
                Remark = entity.Remark 
            };
        }

        public DeviceType ToEntity(DeviceTypeModel model)
        {

            return model == null ? default : new DeviceType
            {
                ConnProtocolCode = model.ConnProtocolCode,
                CreateDate = model.CreateDate,
                Code = model.Code,
                DeviceTypeCode = model.DeviceTypeCode,
                DeviceTypeName= model.DeviceTypeName ,
                ProductCategoryId = model.ProductCategoryId,
                ProtocolCode = model.ProtocolCode,
                OrganizationId = model.OrganizationId,
                UpdateDate = model.UpdateDate,
                Remark = model.Remark, 
                Creater = IdentityContext.Get()?.UserId,
                Updater = IdentityContext.Get()?.UserId,
            };
        }
    }
}

using Kayak.Core.Common.Repsitories;
using Kayak.DataAccess.DeviceData.Entities;
using Kayak.DataAccess.DeviceData;
using Kayak.IModuleServices.DeviceManage.Models;
using Surging.Core.CPlatform.Ioc;
using Microsoft.EntityFrameworkCore;
using Kayak.Core.Common.Context;

namespace Kayak.Modules.DeviceManage.Repositories
{
    public class DeviceConfigRepository : BaseRepository, ISingleInstance
    {
        private readonly IEFRepository<DeviceConfig> _repository;
        public DeviceConfigRepository(IEFRepository<DeviceConfig> repository)
        {
            _repository = repository;
        }

        public async Task<bool> Add(DeviceConfigModel model)
        {
            using (var context = DataContext.Instance())
            {
                context.DeviceConfig.Add(ToEntity(model));
                return await context.SaveChangesAsync() > 0;
            }
        }

        public async Task<DeviceConfigModel> GetByDeviceCode(string deviceCode)
        {
            using (var context = DataContext.Instance())
            {
                var entity = await (from q in context.DeviceConfig where q.DeviceCode == deviceCode select q).FirstOrDefaultAsync();
                return ToModel(entity);
            }
        }

        public async Task<bool> Modify(DeviceConfigModel model)
        {
            using (var context = DataContext.Instance())
            {
                var entity = ToEntity(model);
                var result = _repository.Instance(context).ModifyBy(entity, p => p.DeviceCode == model.DeviceCode, "AuthConfig", "Updater", "UpdateDate") > 0;
                return await Task.FromResult(result);
            }
        }

        public DeviceConfig ToEntity(DeviceConfigModel model)
        {
            return model == null ? default : new DeviceConfig
            {
                AuthConfig = model.AuthConfig, 
                CreateDate = model.CreateDate,
                ProductCode = model.ProductCode,
                DeviceCode = model.DeviceCode,
                UpdateDate = model.UpdateDate,
                Creater = IdentityContext.Get()?.UserId,
                Updater = IdentityContext.Get()?.UserId,
            };
        }

        public DeviceConfigModel ToModel(DeviceConfig entity)
        {
            return entity == null ? default : new DeviceConfigModel
            {
                AuthConfig = entity.AuthConfig,
                CreateDate = entity.CreateDate,
                ProductCode = entity.ProductCode,
                DeviceCode = entity.DeviceCode,
                UpdateDate = entity.UpdateDate,
            };
        }
    }
}

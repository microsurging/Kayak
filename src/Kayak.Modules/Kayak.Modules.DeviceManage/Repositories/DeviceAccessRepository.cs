using Kayak.Core.Common.Repsitories;
using Kayak.DataAccess.DeviceData.Entities;
using Kayak.DataAccess.DeviceData;
using Kayak.IModuleServices.DeviceManage.Models;
using Surging.Core.CPlatform.Ioc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kayak.Core.Common.Response;
using Microsoft.EntityFrameworkCore;
using Kayak.Core.Common.Context;

namespace Kayak.Modules.DeviceManage.Repositories
{
    public class DeviceAccessRepository : BaseRepository, ISingleInstance
    {
        private readonly IEFRepository<DeviceAccess> _repository;
        public DeviceAccessRepository(IEFRepository<DeviceAccess> repository)
        {
            _repository = repository;
        }

        public async Task<bool> Add(DeviceAccessModel model)
        {
            using (var context = DataContext.Instance())
            {
                context.DeviceAccess.Add(ToEntity(model));
                return await context.SaveChangesAsync() > 0;
            }
        }

       public async Task<DeviceAccessModel> GetByProductCode(string productCode)
        {
            using (var context = DataContext.Instance())
            {
              var entity=  await (from q in context.DeviceAccess where q.ProductCode == productCode select q).FirstOrDefaultAsync();
              return  ToModel(entity);
            }
        }

        public async Task<bool> Modify(DeviceAccessModel model)
        {
            using (var context = DataContext.Instance())
            {
                var entity = ToEntity(model);
                var result = _repository.Instance(context).ModifyBy(entity, p => p.ProductCode == model.ProductCode, "AuthConfig", "ConnProtocol", "GatewayName", "GatewayId", "Remark","Updater", "UpdateDate") > 0;
                return await Task.FromResult(result);
            }
        }

        public DeviceAccess ToEntity(DeviceAccessModel model)
        {
            return model == null ? default : new DeviceAccess
            {
                AuthConfig = model.AuthConfig,
                ConnProtocol = model.ConnProtocol,
                CreateDate = model.CreateDate,
                ProductCode = model.ProductCode,
                GatewayName = model.GatewayName,
                Remark=model.Remark,
                GatewayId = model.GatewayId,
                UpdateDate = model.UpdateDate,
                Creater = IdentityContext.Get()?.UserId,
                Updater = IdentityContext.Get()?.UserId,
            };
        }

        public DeviceAccessModel ToModel(DeviceAccess entity)
        {
            return entity == null ? default : new DeviceAccessModel
            {
                Id = entity.Id,
                AuthConfig = entity.AuthConfig,
                ConnProtocol = entity.ConnProtocol,
                CreateDate = entity.CreateDate,
                 Remark=entity.Remark,
                ProductCode = entity.ProductCode,
                GatewayName = entity.GatewayName,
                GatewayId = entity.GatewayId,
                UpdateDate = entity.UpdateDate, 
            };
        }
    }
}

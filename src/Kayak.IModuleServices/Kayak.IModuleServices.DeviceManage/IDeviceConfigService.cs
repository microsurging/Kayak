using Kayak.Core.Common.Response;
using Kayak.IModuleServices.DeviceManage.Models;
using Surging.Core.CPlatform.Filters.Implementation;
using Surging.Core.CPlatform.Ioc;
using Surging.Core.CPlatform.Runtime.Server.Implementation.ServiceDiscovery.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kayak.IModuleServices.DeviceManage
{
    [ServiceBundle("api/{Service}/{Method}")]
    public interface IDeviceConfigService : IServiceKey
    {
        [Authorization(AuthType = AuthorizationType.JWT)]
        Task<ApiResult<bool>> Add(DeviceConfigModel model);

        [Authorization(AuthType = AuthorizationType.JWT)]
        Task<ApiResult<bool>> Modify(DeviceConfigModel model);

        [Authorization(AuthType = AuthorizationType.JWT)]
        Task<ApiResult<DeviceConfigModel>> GetByDeviceCode(string deviceCode);
    }
}

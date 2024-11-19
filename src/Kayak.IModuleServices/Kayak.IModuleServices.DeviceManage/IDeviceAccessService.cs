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
    public interface IDeviceAccessService: IServiceKey
    {
        [Authorization(AuthType = AuthorizationType.JWT)]
        Task<ApiResult<bool>> Add(DeviceAccessModel model);

        [Authorization(AuthType = AuthorizationType.JWT)]
        Task<ApiResult<bool>> Modify(DeviceAccessModel model);

        [Authorization(AuthType = AuthorizationType.JWT)]
        Task<ApiResult<DeviceAccessModel>> GetByProductCode(string productCode);

    }
}

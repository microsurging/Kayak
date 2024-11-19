using Kayak.Core.Common.Response;
using Kayak.IModuleServices.DeviceManage.Models;
using Kayak.IModuleServices.DeviceManage.Queries;
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
    public interface IPropertyConfigureService : IServiceKey
    {
        [Authorization(AuthType = AuthorizationType.JWT)]
        Task<ApiResult<Page<PropertyConfigureModel>>> GetPageAsync(PropertyConfigureQuery query);

        [Authorization(AuthType = AuthorizationType.JWT)]
        Task<ApiResult<bool>> DeleteById(List<int> ids);

        [Authorization(AuthType = AuthorizationType.JWT)]
        Task<ApiResult<bool>> Add(PropertyConfigureModel model);

        [Authorization(AuthType = AuthorizationType.JWT)]
        Task<ApiResult<bool>> Modify(PropertyConfigureModel model);

        [Authorization(AuthType = AuthorizationType.JWT)]
        Task<ApiResult<List<PropertyConfigureModel>>> GetListByDeviceIdAsync(string deviceId,List<string> propertyIds);

        [Authorization(AuthType = AuthorizationType.JWT)]
        Task<ApiResult<PropertyConfigureModel>> GetByProperNameAsync(string correlationId,string correlationFrom, string propertyName);

        [Authorization(AuthType = AuthorizationType.JWT)]
        Task<ApiResult<List<PropertyConfigureModel>>> GetListByIdAsync(List<int> ids);

        [Authorization(AuthType = AuthorizationType.JWT)]
        Task<ApiResult<bool>> Validate(PropertyConfigureModel model);
    }
}

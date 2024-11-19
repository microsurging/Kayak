using Kayak.Core.Common.Response;
using Kayak.IModuleServices.DeviceManage.Models;
using Kayak.IModuleServices.DeviceManage.Queries;
using Surging.Core.CPlatform.Filters.Implementation;
using Surging.Core.CPlatform.Ioc;
using Surging.Core.CPlatform.Runtime.Server.Implementation.ServiceDiscovery.Attributes;
using Surging.Core.ProxyGenerator.Interceptors.Implementation.Metadatas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kayak.IModuleServices.DeviceManage
{
    [ServiceBundle("api/{Service}/{Method}")]
    public  interface IFunctionParameterService:IServiceKey
    {

        [Authorization(AuthType = AuthorizationType.JWT)]
        Task<ApiResult<bool>> Edit(string key, List<FunctionParameterModel> list);

        [Authorization(AuthType = AuthorizationType.JWT)]
        Task<ApiResult<List<FunctionParameterModel>>> GetByFunctionId(int functionId,string parameterType);

        [Authorization(AuthType = AuthorizationType.JWT)]
        Task<ApiResult<List<FunctionParameterModel>>> Get(FunctionParameterQuery query);
    }
}

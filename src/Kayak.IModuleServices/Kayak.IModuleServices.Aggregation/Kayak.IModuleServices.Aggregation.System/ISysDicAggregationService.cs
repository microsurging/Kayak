using Kayak.Core.Common.Response;
using Kayak.IModuleServices.System.Models;
using Surging.Core.CPlatform.Filters.Implementation;
using Surging.Core.CPlatform.Ioc;
using Surging.Core.CPlatform.Runtime.Server.Implementation.ServiceDiscovery.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kayak.IModuleServices.Aggregation.System
{
    [ServiceBundle("api/{Service}/{Method}", isPrefix: true)]
    public interface ISysDicAggregationService : IServiceKey
    {
        [Authorization(AuthType = AuthorizationType.JWT)]
        Task<ApiResult<List<SysDictionaryModel>>> GetSysDictionaryByParentCode(string parentCode);

        [Authorization(AuthType = AuthorizationType.JWT)]
        Task<ApiResult<bool>> Add(SysDictionaryModel model);

        [Authorization(AuthType = AuthorizationType.JWT)]
        Task<ApiResult<bool>> DeleteById(string parentCode,List<int> ids);

        [Authorization(AuthType = AuthorizationType.JWT)]
        Task<ApiResult<bool>> Modify(SysDictionaryModel model);
    }
}

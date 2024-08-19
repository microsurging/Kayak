using Kayak.Core.Common.Response;
using Kayak.IModuleServices.Aggregation.DeviceManage.Models;
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

namespace Kayak.IModuleServices.Aggregation.DeviceManage
{
    [ServiceBundle("api/{Service}/{Method}")]
    public interface IPrdAggregationService : IServiceKey
    {
        [Authorization(AuthType = AuthorizationType.JWT)]
        Task<ApiResult<Page<PrdAggregationModel>>> GetPageAsync(ProductQuery query);

        [Authorization(AuthType = AuthorizationType.JWT)]
        Task<ApiResult<List<ProductModel>>> GetProducts();
    }
}

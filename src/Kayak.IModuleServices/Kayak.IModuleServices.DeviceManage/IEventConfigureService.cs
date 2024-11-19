﻿using Kayak.Core.Common.Response;
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
    public interface IEventConfigureService : IServiceKey
    {
        [Authorization(AuthType = AuthorizationType.JWT)]
        Task<ApiResult<Page<EventConfigureModel>>> GetPageAsync(EventConfigureQuery query);

        [Authorization(AuthType = AuthorizationType.JWT)]
        Task<ApiResult<bool>> DeleteById(List<int> ids);

        [Authorization(AuthType = AuthorizationType.JWT)]
        Task<ApiResult<bool>> Add(EventConfigureModel model);

        [Authorization(AuthType = AuthorizationType.JWT)]
        Task<ApiResult<bool>> Modify(EventConfigureModel model);


        [Authorization(AuthType = AuthorizationType.JWT)]
        Task<ApiResult<bool>> Validate(EventConfigureModel model);
    }
}
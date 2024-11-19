using Kayak.Core.Common.Response;
using Kayak.IModuleServices.Aggregation.ServiceManage;
using Kayak.IModuleServices.ServiceManage;
using Kayak.IModuleServices.ServiceManage.Model;
using Kayak.IModuleServices.ServiceManage.Query;
using Surging.Core.CPlatform;
using Surging.Core.CPlatform.Ioc;
using Surging.Core.CPlatform.Utilities;
using Surging.Core.ProxyGenerator;
using Surging.Core.Stage.Internal;
using Surging.Core.Stage.Internal.Implementation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Kayak.Modules.Aggregation.ServiceManage.Domains
{
    internal class BlackWhiteListAggService : ProxyServiceBase, IBlackWhiteListAggService, ISingleInstance
    {
        private   IIPChecker _ipChecker;
        public BlackWhiteListAggService() { 
        }

        public async Task<ApiResult<bool>> Disable(List<int> ids)
        {
            if (_ipChecker == null)
                _ipChecker = ServiceLocator.GetService<IIPChecker>();
            var blackWhiteList = await GetService<IBlackWhiteListService>().GetListByIds(ids);
            blackWhiteList.Result.ForEach(item =>
            {
                _ipChecker.Remove(new IpCheckerProperties
                {
                    BlackIpAddress = item.BlackList,
                    WhiteIpAddress = item.WhiteList,
                    RoutePathPattern = item.RoutePathPattern
                });
            });
            return await GetService<IBlackWhiteListService>().Disable(ids);
        }

        public async Task<ApiResult<bool>> Enable(List<int> ids)
        {
            if (_ipChecker == null)
                _ipChecker = ServiceLocator.GetService<IIPChecker>();
            var blackWhiteList= await GetService<IBlackWhiteListService>().GetListByIds(ids);
            blackWhiteList.Result.ForEach(item =>
            {
                _ipChecker.AddCheckIpAddresses(new IpCheckerProperties
                {
                    BlackIpAddress = item.BlackList,
                    WhiteIpAddress = item.WhiteList,
                    RoutePathPattern = item.RoutePathPattern
                });
            });
            return await GetService<IBlackWhiteListService>().Enable(ids);
        }

        public async Task<ApiResult<Page<BlackWhiteListModel>>> GetPageAsync(BlackWhiteListQuery query)
        {
            return await GetService<IBlackWhiteListService>().GetPageAsync(query);
        }
    }
}

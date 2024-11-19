using Kayak.IModuleServices.Aggregation.ServiceManage;
using Surging.Core.CPlatform;
using Surging.Core.CPlatform.Engines;
using Surging.Core.CPlatform.Module;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kayak.Modules.Aggregation.ServiceManage
{
    internal class ServiceManageModule : BusinessModule
    {
        public override void Initialize(AppModuleContext context)
        {

            context.ServiceProvoider.GetInstances<IServiceEngineLifetime>().ServiceEngineStarted.Register(() =>
            {
                Task.Factory.StartNew(async () =>
                {
                    var pageCount = 1;
                    var pageSize = 200;
                    for (var page = 1; page <= pageCount; page++)
                    {
                        pageCount = await InitBlackWhite(context.ServiceProvoider, page, pageSize);
                    }
                });
            });
        }
            

        protected override void RegisterBuilder(ContainerBuilderWrapper builder)
        {
        }

       public async Task<int>  InitBlackWhite(CPlatformContainer serviceProvoider, int page, int pageSize)
        {
            var result = await serviceProvoider.GetInstances<IBlackWhiteListAggService>().GetPageAsync(new IModuleServices.ServiceManage.Query.BlackWhiteListQuery
            {
                Status= 1,
                Page = page,
                PageSize = pageSize,
            });
            var ids = result.Result.Items.Select(x => x.Id).ToList();
            await serviceProvoider.GetInstances<IBlackWhiteListAggService>().Enable(ids);
            return result.Result.PageCount;
        }
    }
}

using Kayak.Core.Http.Kestrel;
using Kayak.IModuleServices.Aggregation.DeviceAccess;
using Kayak.IModuleServices.DeviceAccess;
using Kayak.Modules.Aggregation.DeviceAccess.Domains;
using Microsoft.Extensions.DependencyInjection;
using Surging.Core.CPlatform;
using Surging.Core.CPlatform.Engines;
using Surging.Core.CPlatform.Module;
using Surging.Core.KestrelHttpServer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kayak.Modules.Aggregation.DeviceAccess
{
    public class DeviceAccessModule : BusinessModule
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
                    pageCount = await InitProtocol(context.ServiceProvoider, page, pageSize);
                }
                pageCount = 1;
                for (var page = 1; page <= pageCount; page++)
                {
                    pageCount = await InitNetPart(context.ServiceProvoider, page, pageSize);
                }
                pageCount = 1;
                for (var page = 1; page <= pageCount; page++)
                {
                    pageCount = await InitDeviceGateway(context.ServiceProvoider, page, pageSize);
                }
            });
            });
            base.Initialize(context);
        } 

        /// <summary>
        /// Inject dependent third-party components
        /// </summary>
        /// <param name="builder"></param>
        protected override void RegisterBuilder(ContainerBuilderWrapper builder)
        {
        }

        public async Task<int> InitProtocol(CPlatformContainer serviceProvoider, int page, int pageSize)
        {
            var result = await serviceProvoider.GetInstances<IProtocolManageAggService>().GetPageListAsync(new IModuleServices.DeviceAccess.Queries.ProtocolQuery
            {
                Status=1,
                Page = page,
                PageSize = pageSize,
            });
            var ids = result.Result.Items.Select(x => x.Id).ToList();
            await serviceProvoider.GetInstances<IProtocolManageAggService>().Republish(ids);
            return result.Result.PageCount;
        }


        public async Task<int> InitNetPart(CPlatformContainer serviceProvoider, int page, int pageSize)
        {
            var result = await serviceProvoider.GetInstances<INetPartAggregationService>().GetPageListAsync(new IModuleServices.DeviceAccess.Queries.NetworkPartQuery
            {
                Status = 1,
                Page = page,
                PageSize = pageSize,
            });
            var items= result.Result.Items.ToList();
            foreach (var item in items)
                await serviceProvoider.GetInstances<INetPartAggregationService>().CreateNetwork(item.Id);
            return result.Result.PageCount;
        }

        public async Task<int> InitDeviceGateway(CPlatformContainer serviceProvoider, int page, int pageSize)
        {
            var result = await serviceProvoider.GetInstances<IDeviceGatewayAggService>().GetPageListAsync(new IModuleServices.DeviceAccess.Queries.DeviceGatewayQuery
            {
                Status= 1,
                Page = page,
                PageSize = pageSize,
            });
            var ids = result.Result.Items.Select(x => x.Id).ToList();
            await serviceProvoider.GetInstances<IDeviceGatewayAggService>().Open(ids);
            return result.Result.PageCount;
        }
    }
}

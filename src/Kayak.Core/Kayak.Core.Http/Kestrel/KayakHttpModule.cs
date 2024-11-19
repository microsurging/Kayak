using Surging.Core.CPlatform.Module;
using Surging.Core.KestrelHttpServer;
using Surging.Core.KestrelHttpServer.Extensions;

namespace Kayak.Core.Http.Kestrel
{
    public class KayakHttpModule : KestrelHttpModule
    {
        public override void Initialize(AppModuleContext context)
        { 
        }

        /// <summary>
        /// Inject dependent third-party components
        /// </summary>
        /// <param name="builder"></param>
        protected override void RegisterBuilder(ContainerBuilderWrapper builder)
        {
        }
        public override void RegisterBuilder(ConfigurationContext context)
        {
            context.Services.AddFilters(typeof(KayakHttpActionFilter));
            context.Services.AddHttpInterceptors(typeof(KayakHttpInterceptor));
        } 
    }
}

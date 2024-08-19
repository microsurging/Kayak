using Surging.Core.CPlatform.Module;
using Surging.Core.System.Intercept;
using Surging.Core.ProxyGenerator;
using Microsoft.Extensions.Configuration;

namespace Kayak.Core.Common.Intercepte
{
    internal class IntercepteModule : SystemModule
    {
        public override void Initialize(AppModuleContext context)
        {
            base.Initialize(context);
        }

        /// <summary>
        /// Inject dependent third-party components
        /// </summary>
        /// <param name="builder"></param>
        protected override void RegisterBuilder(ContainerBuilderWrapper builder)
        {
            var option = new InterceptorOptions();
            var section = Surging.Core.CPlatform.AppConfig.GetSection("Interceptor");
            if (section.Exists())
                option = section.Get<InterceptorOptions>();
            base.RegisterBuilder(builder);
            if(!option.DisableCache)
            builder.AddClientIntercepted(typeof(CacheProviderInterceptor)); 
            if (!option.DisableOperateLog)
                builder.AddClientIntercepted(typeof(OperateLogInterceptor));
          
        }
    }
}


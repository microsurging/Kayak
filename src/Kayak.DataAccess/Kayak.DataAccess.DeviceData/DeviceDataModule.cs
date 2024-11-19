using Autofac;
using Kayak.DataAccess.UserPermission.Implementation;
using Microsoft.Extensions.Configuration;
using Surging.Core.CPlatform.Module;

namespace Kayak.DataAccess.DeviceData
{
    public class DeviceDataModule : SystemModule
    {
        public override async void Initialize(AppModuleContext context)
        {
            await context.ServiceProvoider.GetInstances<DataContext>("sqlite").InitializeAsync();
            base.Initialize(context);
        }

        /// <summary>
        /// Inject dependent third-party components
        /// </summary>
        /// <param name="builder"></param>
        protected override void RegisterBuilder(ContainerBuilderWrapper builder)
        {
            base.RegisterBuilder(builder);
            var option = new List<DeviceDataOption>();
            var section = Surging.Core.CPlatform.AppConfig.GetSection("DataAccess");
            if (section.Exists())
                option = section.Get<List<DeviceDataOption>>();
            AppConfig.DeviceDataOptions = option.Where(p => p.Name == "DeviceData").FirstOrDefault();
            builder.Register(p => new SqliteContext(AppConfig.DeviceDataOptions?.Connstring)).Named<DataContext>("sqlite");
            //builder.AddClientIntercepted(typeof(LogProviderInterceptor));
        }
    }
}

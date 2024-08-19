using Autofac;
using Kayak.Core.Common.Intercepte.Model;
using Surging.Core.CPlatform.DependencyResolution;
using Surging.Core.CPlatform.Routing;
using Surging.Core.CPlatform.Transport.Implementation;
using Surging.Core.CPlatform.Utilities;
using Surging.Core.ProxyGenerator;
using Surging.Core.ProxyGenerator.Interceptors;
using Surging.Core.ProxyGenerator.Interceptors.Implementation;
using System.Diagnostics;
using System.Text.Json;
namespace Kayak.Core.Common.Intercepte
{
    public class OperateLogInterceptor : IInterceptor
    {
        private readonly IInterceptorProvider _interceptorProvider;

        private readonly IServiceRouteProvider _serviceRouteProvider;
        public OperateLogInterceptor(IInterceptorProvider interceptorProvider, IServiceRouteProvider serviceRouteProvider)
        {
            _interceptorProvider = interceptorProvider;
            _serviceRouteProvider = serviceRouteProvider;
        }

        public async Task Intercept(IInvocation invocation)
        {
            var route = await _serviceRouteProvider.Locate(invocation.ServiceId);
            var cacheMetadata = route.ServiceDescriptor.GetIntercept("Log");
            if (cacheMetadata != null)
            {
                var result = invocation.ReturnValue;
                var watch = Stopwatch.StartNew();
                if(result ==null)
                await invocation.Proceed();
                watch.Stop();
                  result = invocation.ReturnValue;
                var model = new OperateLogModel()
                {
                    Arguments = JsonSerializer.Serialize(invocation.Arguments),
                    CreateDate = DateTime.Now,
                    ReturnType = invocation.ReturnType.Name ?? "",
                    RoutePath = route?.ServiceDescriptor.RoutePath ?? "",
                    ServiceId = invocation.ServiceId,
                    ReturnValue = JsonSerializer.Serialize(result),
                    Payload = RpcContext.GetContext().GetAttachment("payload")?.ToString(),
                    RunTime= watch.ElapsedMilliseconds

                };
                await GetService<IOperateLogService>().Add(model);
            }
        }


        private  T GetService<T>() where T : class
        {
            if (ServiceLocator.Current.IsRegistered<T>())
                return ServiceLocator.GetService<T>();
            else
            {
                var result = ServiceResolver.Current.GetService<T>();
                if (result == null)
                {
                    result = ServiceLocator.GetService<IServiceProxyFactory>().CreateProxy<T>();
                    ServiceResolver.Current.Register(null, result);
                }
                return result;
            }

        }
    }
}

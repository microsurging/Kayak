using Surging.Core.CPlatform;
using Surging.Core.CPlatform.Protocol;
using Surging.Core.CPlatform.Utilities;
using Surging.Core.DeviceGateway.Runtime.Core;
using Surging.Core.DeviceGateway.Runtime.Device.Implementation;
using Surging.Core.DeviceGateway.Utilities;
using Surging.Core.KestrelHttpServer.Interceptors;
using Surging.Core.KestrelHttpServer.Interceptors.Implementation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kayak.Core.Http.Kestrel
{
    public class KayakHttpInterceptor : IHttpInterceptor
    {
        private readonly IDeviceGatewayManage _deviceGatewayManage;
        private readonly IProtocolSupports _protocolSupports;
        public KayakHttpInterceptor() {
            _deviceGatewayManage = ServiceLocator.GetService<IDeviceGatewayManage>();
            _protocolSupports = ServiceLocator.GetService<IProtocolSupports>();

        }


        public async Task<bool> Intercept(IHttpInvocation invocation)
        {
            var result = false;
            var httpInvocation = invocation as HttpInvocation;
            HttpDeviceInvocation deviceInvocation = new HttpDeviceInvocation
            {
                Context = invocation.Context,
                Entry = invocation.Entry,
                Path = invocation.Path,
                NetworkId = invocation.NetworkId,
                Sender = httpInvocation?.Sender
            };
            deviceInvocation.DeviceGatewayProperties = _deviceGatewayManage.GetGatewayProperties().Where(p => p.ChannelId == invocation.NetworkId).FirstOrDefault();
            if (deviceInvocation.DeviceGatewayProperties != null)
            {
                var protocolSupport = await _protocolSupports.GetProtocol(deviceInvocation.DeviceGatewayProperties.Protocol);
                deviceInvocation.ProtocolSupport = (ProtocolSupport)protocolSupport;
                deviceInvocation.DeviceGateway = await _deviceGatewayManage.GetGateway(deviceInvocation.DeviceGatewayProperties.Id).FirstOrDefaultAsync();
                var complexProtocolSupport = protocolSupport as ComplexProtocolSupport;
                var routes = await complexProtocolSupport.GetRoutes(MessageTransport.Http);
                var route = routes.Where(route => PathUtils.Match(route.RoutePath, deviceInvocation.Path)).FirstOrDefault();
                result = route != null;
                if (result &&  (!string.IsNullOrEmpty(route.HttpMethod())
                    && !route.HttpMethod().Equals(deviceInvocation.Context.Request.Method, StringComparison.OrdinalIgnoreCase)))
                {
                    result = false;
                    return result;
                }
            }
            result = await deviceInvocation.Proceed();
            invocation.Result = deviceInvocation.Result;
            return result;
        }
    }
}

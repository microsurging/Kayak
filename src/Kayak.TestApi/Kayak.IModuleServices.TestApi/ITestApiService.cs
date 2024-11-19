using Surging.Core.CPlatform.Filters.Implementation;
using Surging.Core.CPlatform.Ioc;
using Surging.Core.CPlatform.Runtime.Client.Address.Resolvers.Implementation.Selectors.Implementation;
using Surging.Core.CPlatform.Runtime.Server.Implementation.ServiceDiscovery.Attributes;
using Surging.Core.CPlatform.Support.Attributes;

namespace Kayak.IModuleServices.TestApi
{
    [ServiceBundle("api/{Service}/{Method}")]
    public interface ITestApiService:IServiceKey
    {
        // [Authorization(AuthType = AuthorizationType.JWT)]
        [Command(ShuntStrategy =AddressSelectorMode.RoundRobin)]
        public Task<string> SayHello(string name);
    }
}

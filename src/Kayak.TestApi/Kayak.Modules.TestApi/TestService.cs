
using Kayak.IModuleServices.TestApi;
using Surging.Core.CPlatform.Ioc;
using Surging.Core.ProxyGenerator;

namespace Kayak.Modules.TestApi
{
    public class TestService : ProxyServiceBase, ITestApiService, ISingleInstance
    {
        public Task<string> SayHello(string name)
        {
            return Task.FromResult($"{name} say:hello world");
        }
    }
}

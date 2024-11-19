using Surging.Apm.Skywalking.Abstractions;

namespace Surging.Apm.Skywalking.Core.Common
{
    internal class HostingEnvironmentProvider : IEnvironmentProvider
    {
        public string EnvironmentName { get; }

        public HostingEnvironmentProvider()
        {
            EnvironmentName ="";
        }
    }
}

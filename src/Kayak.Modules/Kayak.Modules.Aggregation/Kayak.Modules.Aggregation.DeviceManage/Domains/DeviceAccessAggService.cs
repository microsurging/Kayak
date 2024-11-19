using Kayak.Core.Common;
using Kayak.Core.Common.Response;
using Kayak.IModuleServices.Aggregation.DeviceManage;
using Kayak.IModuleServices.Aggregation.DeviceManage.Models;
using Kayak.IModuleServices.DeviceAccess;
using Kayak.IModuleServices.DeviceAccess.Models;
using Kayak.IModuleServices.DeviceManage;
using Kayak.IModuleServices.DeviceManage.Models;
using Kayak.IModuleServices.System; 
using MongoDB.Driver.Core.Clusters;
using Surging.Core.Caching;
using Surging.Core.CPlatform;
using Surging.Core.CPlatform.Engines;
using Surging.Core.CPlatform.Engines.Implementation;
using Surging.Core.CPlatform.Ioc;
using Surging.Core.CPlatform.Protocol;
using Surging.Core.DeviceGateway.Runtime.Core;
using Surging.Core.DeviceGateway.Runtime.Core.Metadata;
using Surging.Core.DeviceGateway.Runtime.Device.Implementation;
using Surging.Core.KestrelHttpServer;
using Surging.Core.ProxyGenerator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kayak.Modules.Aggregation.DeviceManage.Domains
{
    internal class DeviceAccessAggService : ProxyServiceBase, IDeviceAccessAggService, ISingleInstance
    {
        private readonly VirtualPathProviderServiceEngine _serviceEngine;
        private readonly IProtocolSupports _protocolSupports;
        public DeviceAccessAggService(IProtocolSupports protocolSupports, IServiceEngine serviceEngine)
        {
            _protocolSupports = protocolSupports;
            _serviceEngine = serviceEngine as VirtualPathProviderServiceEngine;

        }

        public async Task<ApiResult<ConfigMetadata>> GetPropertyConfig(string productCode)
        {
            ConfigMetadata configMetadata = null;
            var deviceAccess = await GetService<IDeviceAccessService>().GetByProductCode(productCode);
            if (deviceAccess.Result != null)
            {
                var deviceGateway = await GetService<IDeviceGatewayService>().GetModelById(deviceAccess.Result.GatewayId);
                var protocol = await GetService<IProtocolSupports>().GetProtocol(deviceGateway.Result.ProtocolCode).GetAwaiter() as ProtocolSupport;
                var networkPart = await GetService<INetworkPartService>().GetNetworkPartById(deviceGateway.Result.NetWorkId);
                configMetadata = await protocol?.GetConfigMetadata(Enum.Parse<MessageTransport>(networkPart.Result.ComponentTypeCode));
            }
            return ApiResult<ConfigMetadata>.Succeed(configMetadata);
        }

        public async Task<ApiResult<List<ServiceDescriptor>>> GetRoutes(string productCode)
        {
            List<ServiceDescriptor>  serviceDescriptors  = new List<ServiceDescriptor>();
            var deviceAccess = await GetService<IDeviceAccessService>().GetByProductCode(productCode);
            if (deviceAccess.Result != null)
            {
                var deviceGateway = await GetService<IDeviceGatewayService>().GetModelById(deviceAccess.Result.GatewayId);
                var protocol = await GetService<IProtocolSupports>().GetProtocol(deviceGateway.Result.ProtocolCode).GetAwaiter() as ProtocolSupport;
                var networkPart = await GetService<INetworkPartService>().GetNetworkPartById(deviceGateway.Result.NetWorkId);
                serviceDescriptors = await protocol?.GetRoutes(Enum.Parse<MessageTransport>(networkPart.Result.ComponentTypeCode));
            }
            return ApiResult<List<ServiceDescriptor>>.Succeed(serviceDescriptors);
        }

        public async Task<IActionResult> DownDocumentFile(string productCode)
        {
            var rootpath = "";
            string uploadPath = "";
            var componentServiceLocation = "";
            if (_serviceEngine != null &&
                    _serviceEngine.ComponentServiceLocationFormats != null)
            {
                componentServiceLocation = _serviceEngine.ComponentServiceLocationFormats[0];
            }
            var deviceAccess = await GetService<IDeviceAccessService>().GetByProductCode(productCode);
            if (deviceAccess.Result != null)
            {
                var deviceGateway = await GetService<IDeviceGatewayService>().GetModelById(deviceAccess.Result.GatewayId);
                var protocolModel = await GetService<IProtocolManageService>().GetProtocolByCode(deviceGateway.Result.ProtocolCode);
                var protocol = await GetService<IProtocolSupports>().GetProtocol(deviceGateway.Result.ProtocolCode).GetAwaiter() as ProtocolSupport;
                var networkPart = await GetService<INetworkPartService>().GetNetworkPartById(deviceGateway.Result.NetWorkId);
                var document = protocol?.GetDocument(Enum.Parse<MessageTransport>(networkPart.Result.ComponentTypeCode)); 
                if (Enum.Parse<ProtocolType>(protocolModel.Result.ProtocolType) == ProtocolType.LocalModule)
                {
                    rootpath = protocolModel.Result.FileAddress == "/" ? AppContext.BaseDirectory : protocolModel.Result.FileAddress;
                    uploadPath = Path.Combine(rootpath,  document ?? "");
                }
                else if (Enum.Parse<ProtocolType>(protocolModel.Result.ProtocolType) == ProtocolType.ModuleFile)
                {
                    rootpath = Surging.Core.CPlatform.AppConfig.ServerOptions.RootPath;
                    uploadPath = Path.Combine(rootpath, componentServiceLocation, deviceGateway.Result.ProtocolCode, document ?? "");
                }
            }
            var fileName = Path.GetFileName(uploadPath);
            if (File.Exists(uploadPath))
            {
                using (var stream = new FileStream(uploadPath, FileMode.Open))
                {
                    var bytes = new Byte[stream.Length];
                    await stream.ReadAsync(bytes, 0, bytes.Length);
                    stream.Dispose();
                    return new FileContentResult(bytes, "application/markdown", fileName);
                }
            }
            else
            {
                throw new FileNotFoundException(fileName);
            }
        }

        public async Task<ApiResult<DeviceAccessAggModel>> GetByProductCode(string productCode)
        {
            DeviceAccessAggModel result = null;
            var deviceAccess = await GetService<IDeviceAccessService>().GetByProductCode(productCode);
            if (deviceAccess.Result != null)
            {
                var gateway = await GetService<IDeviceGatewayService>().GetModelById(deviceAccess.Result.GatewayId);
                var networkParts = await GetService<INetworkPartService>().GetListByIds(new List<int> { gateway.Result.NetWorkId });
                var protocols = await GetService<IProtocolManageService>().GetProtocols();
                result = new DeviceAccessAggModel()
                {
                    AuthConfig = deviceAccess.Result.AuthConfig,
                    ConnProtocol = deviceAccess.Result.ConnProtocol,
                    CreateDate = deviceAccess.Result.CreateDate,
                    GatewayId = deviceAccess.Result.GatewayId,
                    GatewayName = deviceAccess.Result.GatewayName,
                    ProductCode = deviceAccess.Result.ProductCode,
                    Remark = deviceAccess.Result.Remark,
                    Id = deviceAccess.Result.Id,
                    UpdateDate = deviceAccess.Result.UpdateDate,
                    NetworkPart = networkParts.Result.Where(p => p.Id == gateway.Result.NetWorkId).Select(p => new NetworkPartModel
                    {
                        Id = p.Id,
                        Name = p.Name,
                        ComponentTypeCode = p.ComponentTypeCode,
                        Host = p.Host,
                        Port = p.Port,
                    }).FirstOrDefault(),
                    Protocol = protocols.Result.Where(p => p.ProtocolCode == gateway.Result.ProtocolCode).Select(p => new ProtocolModel
                    {
                        Id = p.Id,
                        ProtocolName = p.ProtocolName,
                        ProtocolCode = p.ProtocolCode,
                        ConnProtocol = p.ConnProtocol

                    }).FirstOrDefault(),
                };
            }
            return ApiResult<DeviceAccessAggModel>.Succeed(result);
        }
    }
}

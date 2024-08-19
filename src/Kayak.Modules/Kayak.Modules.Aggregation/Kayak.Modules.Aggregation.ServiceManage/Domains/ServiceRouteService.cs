using Kayak.Core.Common.Response;
using Kayak.IModuleServices.Aggregation.ServiceManage;
using Kayak.IModuleServices.ServiceManage;
using Kayak.IModuleServices.ServiceManage.Query;
using Microsoft.Extensions.Logging;
using Surging.Core.Consul;
using Surging.Core.Consul.Internal;
using Surging.Core.Consul.WatcherProvider;
using Surging.Core.CPlatform.Ioc;
using Surging.Core.CPlatform.Routing;
using Surging.Core.CPlatform.Runtime.Client;
using Surging.Core.CPlatform.Serialization;
using Surging.Core.ProxyGenerator;
using Kayak.Core.Common.Extensions;
using Kayak.IModuleServices.Aggregation.ServiceManage.Models;
using Kayak.IModuleServices.Aggregation.ServiceManage.Queries;
using System.Collections.Concurrent;
using Kayak.IModuleServices.ServiceManage.Model;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using Surging.Core.CPlatform;
using Surging.Core.CPlatform.Address;
using Surging.Core.CPlatform.Runtime.Client.HealthChecks;
using Surging.Core.CPlatform.Runtime.Server;
using Surging.Core.CPlatform.Support;
using Surging.Core.CPlatform.Utilities;

namespace Kayak.Modules.Aggregation.ServiceManage.Domains
{
    internal class ServiceRouteService : ProxyServiceBase, IServiceRouteService, ISingleInstance
    {
        private readonly ISerializer<byte[]> _serializer;
        private readonly IServiceRouteFactory _serviceRouteFactory;
        private readonly ILogger<ConsulServiceRouteManager> _logger;
        private readonly ILogger<ConsulServiceCommandManager> _commandLogger;
        private readonly ISerializer<string> _stringSerializer;
        private readonly IClientWatchManager _manager;
        private ServiceRoute[] _routes;
        private readonly IConsulClientProvider _consulClientProvider;
        private readonly IServiceHeartbeatManager _serviceHeartbeatManager;
        private readonly IHealthCheckService _healthCheckService;
        private readonly IServiceEntryManager _serviceEntryManager;
        private ConcurrentDictionary<int, IServiceCommandManager> _serviceCommandManagers;
        private ConcurrentDictionary<int, IServiceRouteManager> _serviceRouteManagers;
        public ServiceRouteService(ISerializer<byte[]> serializer,
       ISerializer<string> stringSerializer, IClientWatchManager manager, IServiceRouteFactory serviceRouteFactory,
       ILogger<ConsulServiceRouteManager> logger,
       ILogger<ConsulServiceCommandManager> commandLogger,
       IServiceHeartbeatManager serviceHeartbeatManager, IServiceEntryManager serviceEntryManager, IConsulClientProvider consulClientProvider, IHealthCheckService healthCheckService) {
            _serializer = serializer;
            _stringSerializer = stringSerializer;
            _serviceRouteFactory = serviceRouteFactory;
            _logger = logger;
            _consulClientProvider = consulClientProvider;
            _manager = manager;
            _commandLogger = commandLogger;
            _healthCheckService = healthCheckService;
            _serviceHeartbeatManager = serviceHeartbeatManager;
            _serviceEntryManager = serviceEntryManager;
            _serviceRouteManagers = new ConcurrentDictionary<int, IServiceRouteManager>();
            _serviceCommandManagers = new ConcurrentDictionary<int, IServiceCommandManager>();
        }
        public async Task<ApiResult<Page<ServiceRoute>>> GetPageAsync(ServiceRouteQuery query)
        {
            var result = new Page<ServiceRoute>();
            var routeManage = await GetServiceRouteManager(query.RegistryCenterType??0);
            if (routeManage != null)
            {
                var serviceRoutes = await routeManage.GetRoutesAsync();
                var total = serviceRoutes.Count();
                  result = new Page<ServiceRoute>()
                {
                    Items = serviceRoutes.Skip((query.Page - 1) * query.PageSize).Take(query.PageSize).ToList(),
                    PageCount = total.GetCeiling(query.PageSize),
                    PageIndex = query.Page,
                    PageSize = query.PageSize,
                    Total = total
                };
            }
            return ApiResult<Page<ServiceRoute>>.Succeed(result);
        }

        public async Task<ApiResult<ServiceStatisticsModel>> GetStatistics(int registryCenterType)
        {
            var result = new ServiceStatisticsModel();
            var routeManage = await GetServiceRouteManager(registryCenterType);
            if (routeManage != null) {
                var serviceRoutes = await routeManage.GetRoutesAsync();
                var addressModels = await routeManage.GetAddressAsync();
                result = new ServiceStatisticsModel
                {
                    ServiceTotal = serviceRoutes.Count(),
                    ServiceRunCount = serviceRoutes.Where(p => p.Address.Count() >= 1).Count(),
                    ServiceAbnormalCount = serviceRoutes.Where(p => p.Address.Count() == 0).Count(),
                    ServiceNodeCount = addressModels.Count()
                };
            }
           return ApiResult<ServiceStatisticsModel>.Succeed(result);
        }

        public async Task<IServiceRouteManager?> GetServiceRouteManager(int registryCenterType)
        {
            var result = new ServiceStatisticsModel();
            var registryCenter = new RegistryCenterModel();
            if (!_serviceRouteManagers.ContainsKey(registryCenterType))
            {
                var registryCenterList = await GetService<IRegistryCenterService>().GetList();
                registryCenter = registryCenterList.Result.Where(p => p.Id == registryCenterType).FirstOrDefault();
            }
            if (registryCenter != null)
            {

                var serviceRouteManager = _serviceRouteManagers.GetOrAdd(registryCenterType, (p) =>
                {
                    var configInfo = new Surging.Core.Consul.Configurations.ConfigInfo($"{registryCenter.Host}:{registryCenter.Port}");
                    return new ConsulServiceRouteManager(configInfo, _serializer, _stringSerializer, _manager, _serviceRouteFactory, _logger, _serviceHeartbeatManager, _consulClientProvider);
                });
                return serviceRouteManager;
            }
            return default;
        }

        public async Task<IServiceCommandManager?> GetServiceCommandManager(int registryCenterType)
        {
            var result = new ServiceStatisticsModel();
            var registryCenter = new RegistryCenterModel();
            if (!_serviceRouteManagers.ContainsKey(registryCenterType))
            {
                var registryCenterList = await GetService<IRegistryCenterService>().GetList();
                registryCenter = registryCenterList.Result.Where(p => p.Id == registryCenterType).FirstOrDefault();
            }
            if (registryCenter != null)
            {
                var routeManage=await GetServiceRouteManager(registryCenterType);
                var serviceRouteManager = _serviceCommandManagers.GetOrAdd(registryCenterType, (p) =>
                {
                    var configInfo = new Surging.Core.Consul.Configurations.ConfigInfo($"{registryCenter.Host}:{registryCenter.Port}");
                    return new ConsulServiceCommandManager(configInfo, _serializer, _stringSerializer, routeManage, _manager,_serviceEntryManager, _commandLogger, _serviceHeartbeatManager, _consulClientProvider);
                });
                return serviceRouteManager;
            }
            return default;
        }

        public async Task<ApiResult<ServiceDescriptor>> GetServiceDescriptor(int registryCenterType, string serviceId)
        {
            var result = new ServiceDescriptor();
            var routeManage = await GetServiceRouteManager(registryCenterType);
            if (routeManage != null)
            {
                var serviceRoute = await routeManage.GetAsync(serviceId);
                if (serviceRoute != null)
                {
                    result = serviceRoute.ServiceDescriptor;
                }
            }
            return ApiResult<ServiceDescriptor>.Succeed(result);
        }

        public async Task<ApiResult<ServiceCommand>> GetServiceCommand(int registryCenterType, string serviceId)
        {
            var result = new ServiceCommand();
            var commandManage = await GetServiceCommandManager(registryCenterType);
            if (commandManage != null)
            {
                var serviceCommand = await commandManage.GetServiceCommandsAsync(serviceId);

                result = serviceCommand.FirstOrDefault();
            }
            return ApiResult<ServiceCommand>.Succeed(result);
        }

        public async Task<ApiResult<Page<CheckIpAddressModel>>> GetAddresses(ServiceRouteQuery query)
        {
           var result = new Page<CheckIpAddressModel>();
            var routeManage = await GetServiceRouteManager(query.RegistryCenterType??0);
            if (routeManage != null)
            {
                var  addresses=  await routeManage.GetAddressAsync(query.serviceId);
                var total = addresses.Count();
                var list = addresses.Skip((query.Page - 1) * query.PageSize).Take(query.PageSize).Select(  p => new CheckIpAddressModel
                {

                    Address = p as IpAddressModel, 
                }).ToList();
                list.ForEach(async p =>
                {
                    p.IsHealth = await _healthCheckService.IsHealth(p.Address);
                });
                result = new Page<CheckIpAddressModel>()
                {
                    Items = list,
                    PageCount = total.GetCeiling(query.PageSize),
                    PageIndex = query.Page,
                    PageSize = query.PageSize,
                    Total = total
                }; 
            }
            return ApiResult<Page<CheckIpAddressModel>>.Succeed(result);
        }

        public async Task<ApiResult<bool>> ModifyAddress(ModifyAddressParams param)
        { 
            var routeManage = await GetServiceRouteManager(param.RegistryCenterType??0);
            if (routeManage != null)
            {
                var serviceRoute = await routeManage.GetAsync(param.ServiceId);
                if (serviceRoute != null)
                {
                    foreach (var item in serviceRoute.Address)
                    {
                        if(item.ToString()==param.ToString())
                        item.Weight = param.Weight;
                    }
                  await  routeManage.SetRoutesAsync(new List<ServiceRoute>() { serviceRoute });
                }
            }
            return ApiResult<bool>.Succeed(true);
        }

        public async Task<ApiResult<bool>> ModifyServiceDescriptor(EditServiceDescriptor model)
        {
            var routeManage = await GetServiceRouteManager(model.RegistryCenterType ?? 0);
            if (routeManage != null)
            {
                var serviceRoute = await routeManage.GetAsync(model.Id);
                if (serviceRoute != null)
                { 
                        if (!model.Token.IsNullOrEmpty())
                        serviceRoute.ServiceDescriptor.Token = model.Token;
                    if(model.Metadatas.Count()>0)
                    {
                        serviceRoute.ServiceDescriptor.Metadatas = model.Metadatas;
                    }
                    await routeManage.SetRoutesAsync(new List<ServiceRoute>() { serviceRoute });
                }
            }
            return ApiResult<bool>.Succeed(true);
        }

        public async Task<ApiResult<bool>> AddServiceDescriptor(EditServiceDescriptor model)
        {
            var routeManage = await GetServiceRouteManager(model.RegistryCenterType ?? 0);
            if (routeManage != null)
            {

                var serviceRoute = new ServiceRoute()
                {
                    ServiceDescriptor = new ServiceDescriptor
                    {
                        Id = model.Id,
                        RoutePath = model.RoutePath,
                        Token = model.Token,
                        Metadatas = new Dictionary<string, object>()
                    }
                };
                await routeManage.SetRoutesAsync(new List<ServiceRoute>() { serviceRoute });
                routeManage.ClearRoute();
            }
            return ApiResult<bool>.Succeed(true);
        }

       
    }
}

using Kayak.Core.Common.Extensions;
using Kayak.Core.Common.Response;
using Kayak.IModuleServices.Aggregation.ServiceManage;
using Kayak.IModuleServices.Aggregation.ServiceManage.Models;
using Kayak.IModuleServices.Aggregation.ServiceManage.Queries;
using Surging.Core.CPlatform;
using Surging.Core.CPlatform.Ioc;
using Surging.Core.CPlatform.Routing;
using Surging.Core.CPlatform.Runtime.Server;
using Surging.Core.ProxyGenerator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kayak.Modules.Aggregation.ServiceManage.Domains
{
    internal class IntermediateServiceService : ProxyServiceBase, IIntermediateServiceService, ISingleInstance
    {
        private readonly IServiceEntryManager _serviceEntryManager;
        public IntermediateServiceService(IServiceEntryManager serviceEntryManager) {

            _serviceEntryManager = serviceEntryManager;
        }

        public Task<ApiResult<ServiceEntryModel>> GetByServiceId(string serviceId)
        {
             
            var allServiceEntries = _serviceEntryManager.GetAllEntries().ToList();
            var serviceEntries = _serviceEntryManager.GetEntries().ToList();
            var result = allServiceEntries.Where(p => p.Descriptor.Id == serviceId).Select(p => new ServiceEntryModel
            {
                //  Attributes = p.Attributes,
                Descriptor = p.Descriptor,
                IsPermission = p.IsPermission,
                ModuleName = p.Type.Assembly.GetName().Name,
                MethodName = p.MethodName,
                Methods = p.Methods,
                RoutePath = p.RoutePath,
                ServiceType = serviceEntries.Any(m => m.RoutePath == p.RoutePath) ? ServiceType.AggregationService : ServiceType.MicroService
            }).FirstOrDefault();
           
            return Task.FromResult(ApiResult<ServiceEntryModel>.Succeed(result));
        }

        public Task<ApiResult<Page<ServiceEntryModel>>> GetPageAsync(IntermediateServiceQuery query)
        {
            var serviceEntries = _serviceEntryManager.GetEntries().ToList();
            var allServiceEntries = _serviceEntryManager.GetAllEntries().ToList();
            if (!query.ServiceType.IsNullOrEmpty())
            {
                var serviceType = Enum.Parse<ServiceType>(query.ServiceType);
                if (serviceType == ServiceType.MicroService)
                {
                    var routepaths = serviceEntries.Select(p => p.RoutePath).ToList();
                    allServiceEntries = allServiceEntries.Where(p => !routepaths.Contains(p.RoutePath)).ToList();
                }
                if (serviceType == ServiceType.AggregationService) allServiceEntries = serviceEntries;
            }
                allServiceEntries = allServiceEntries
                             .WhereIF(query.ServiceId != null, e => e.Descriptor.Id == query.ServiceId)
                             .WhereIF(!query.RoutePath.IsNullOrEmpty(), e => e.RoutePath == query.RoutePath)
                             .WhereIF(!query.ModuleName.IsNullOrEmpty(), e => e.Type.Assembly.GetName().Name.Contains(query.ModuleName)).ToList();
            var total = allServiceEntries.Count();
            var result = new Page<ServiceEntryModel>()
            {
                Items = allServiceEntries.Skip((query.Page - 1) * query.PageSize).Take(query.PageSize).Select(p => new ServiceEntryModel
                {
                  //  Attributes = p.Attributes,
                    Descriptor = p.Descriptor,
                    IsPermission = p.IsPermission,
                    ModuleName=p.Type.Assembly.GetName().Name,
                    MethodName = p.MethodName,
                    Methods = p.Methods, 
                    RoutePath = p.RoutePath,
                     ServiceType= serviceEntries.Any(m=>m.RoutePath==p.RoutePath)?ServiceType.AggregationService:ServiceType.MicroService
                }).ToList(),
                PageCount = total.GetCeiling(query.PageSize),
                PageIndex = query.Page,
                PageSize = query.PageSize,
                Total = total
            };

            return Task.FromResult(ApiResult<Page<ServiceEntryModel>>.Succeed(result));
        }

        public  Task<ApiResult<ServiceDescriptor>> GetServiceDescriptor(string serviceId)
        {
            var result = new ServiceDescriptor();
            var serviceEntries = _serviceEntryManager.GetAllEntries().ToList();

            var serviceEntry =  serviceEntries.Where(p=>p.Descriptor.Id== serviceId).FirstOrDefault();
            if (serviceEntry != null)
            {
                result = serviceEntry.Descriptor;
            }

            return Task.FromResult(ApiResult<ServiceDescriptor>.Succeed(result));
        }
    }
}

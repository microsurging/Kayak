using Kayak.Core.Common;
using Kayak.Core.Common.Response;
using Kayak.IModuleServices.Aggregation.DeviceAccess;
using Kayak.IModuleServices.Aggregation.DeviceAccess.Models;
using Kayak.IModuleServices.DeviceAccess;
using Kayak.IModuleServices.DeviceAccess.Models;
using Kayak.IModuleServices.DeviceAccess.Queries;
using Kayak.IModuleServices.System;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.DependencyInjection;
using Surging.Core.CPlatform.Ioc;
using Surging.Core.CPlatform.Network;
using Surging.Core.CPlatform.Transport.Implementation;
using Surging.Core.CPlatform.Utilities;
using Surging.Core.KestrelHttpServer.Internal;
using Surging.Core.ProxyGenerator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Reactive.Subjects;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Kayak.Modules.Aggregation.DeviceAccess.Domains
{
    internal class NetPartAggregationService : ProxyServiceBase, INetPartAggregationService, ISingleInstance
    {
        private readonly INetworkManager _networkManager;
        public NetPartAggregationService(INetworkManager networkManager) {
            _networkManager = networkManager;
        }

        public async Task<ApiResult<bool>> CreateNetwork(int id)
        { 
            var httpHeaders = RestContext.GetContext().GetAttachment("HttpHeaders");
            var apiResult = await GetService<INetworkPartService>().GetNetworkPartById(id);
            var model = apiResult.Result;  
            if (model != null )
            { 
                var networkProp = GetNetworkProperties(model, id);
                var subject = new Subject<NetworkLogMessage>();
                subject.Subscribe(p => GetService<INetworkLogService>().Add(new IModuleServices.System.Models.NetworkLogModel
                {
                    Content = p.Content,
                    CreateDate = p.CreateDate,
                    EventName = p.EventName,
                    NetworkId = p.Id,
                    logLevel = p.logLevel,
                    NetworkType = p.NetworkType
                }));
                var provider = ServiceLocator.GetService<INetworkProvider<NetworkProperties>>((Enum.Parse<NetworkType>(model.ComponentTypeCode)).ToString());
                _networkManager.CreateOrUpdate(provider, networkProp, subject);
                await GetService<INetworkPartService>().Open(new List<int> { id });
            }
            return ApiResult<bool>.Succeed(true);
        }

        public async Task<ApiResult<bool>> ShundownNetwork(int id)
        {
           
            var apiResult = await GetService<INetworkPartService>().GetNetworkPartById(id);
            var model = apiResult.Result;
            if (model != null)
            {
                _networkManager.Shutdown(Enum.Parse<NetworkType>(model.ComponentTypeCode), id.ToString());
              // ServiceLocator.GetService<INetworkProvider<NetworkProperties>>(((NetworkType)model.ComponentTypeId).ToString())?.Shutdown(id.ToString());
                await GetService<INetworkPartService>().Stop(new List<int> { id });
            }
            return ApiResult<bool>.Succeed(true);
        }

        public async Task<ApiResult<Page<NetPartAggregationModel>>> GetPageAsync(NetworkPartQuery query)
        {

            var apiResult = await GetService<INetworkPartService>().GetPageAsync(query);
            var commponenttypes = await GetService<IModuleServices.System.ISysDictionaryService>().GetSysDictionaryByParentCode(SysDictionaryCodes.COMPONENTTYPE);
            var clustermodes = await GetService<IModuleServices.System.ISysDictionaryService>().GetSysDictionaryByParentCode(SysDictionaryCodes.CLUSTERMODE);
            Page<NetPartAggregationModel> pageModel = new Page<NetPartAggregationModel>()
            {
                PageCount = apiResult.Result.PageCount,
                PageIndex = apiResult.Result.PageIndex,
                PageSize = apiResult.Result.PageSize,
                Total = apiResult.Result.Total,
                Items = apiResult.Result.Items.Select(x => new NetPartAggregationModel
                {
                    ComponentTypeCode = x.ComponentTypeCode,
                    ClusterModeId = x.ClusterModeId,
                    Id = x.Id,
                    CreateDate = x.CreateDate,
                    Delimited = x.Delimited,
                    EnableTLS = x.EnableTLS,
                    FixedLength = x.FixedLength,
                    EnableSwagger = x.EnableSwagger,
                    EnableWebService = x.EnableWebService,
                    MaxMessageLength = x.MaxMessageLength,
                     IsMulticast = x.IsMulticast,
                    ResolveMode = x.ResolveMode,
                    EnableSSL = x.EnableSSL,
                    Host = x.Host,
                    Name = x.Name,
                    Port = x.Port,
                    RuleScript = x.RuleScript,
                    Status = x.Status,
                    ClusterMode = clustermodes.Result.Where(p => p.Value == x.ClusterModeId).Select(p => new NetPartDictionary
                    {
                        Code = p.Code,
                        Name = p.Name,
                        Value = p.Value,
                    }).FirstOrDefault(),
                    ComponentType = commponenttypes.Result.Where(p => p.Code == x.ComponentTypeCode).Select(p => new NetPartDictionary
                    {
                        Code = p.Code,
                        Name = p.Name,
                        Value = p.Value,
                    }).FirstOrDefault(),
                    Remark = x.Remark,
                    UpdateDate = x.UpdateDate,


                }).ToList()
            };
            return ApiResult<Page<NetPartAggregationModel>>.Succeed(pageModel);
        }

        public async Task<ApiResult<bool>> Modify(NetworkPartModel model)
        {
            ServiceLocator.GetService<INetworkProvider<NetworkProperties>>((Enum.Parse<NetworkType>(model.ComponentTypeCode)).ToString())?.Shutdown(model.Id.ToString());
            return  await GetService<INetworkPartService>().Modify(model);
        }

        public async Task<ApiResult<Page<NetworkPartModel>>> GetPageListAsync(NetworkPartQuery query)
        {
            var result = await GetService<INetworkPartService>().GetPageAsync(query);
            return result;
        }

        private NetworkProperties GetNetworkProperties(NetworkPartModel? model,int id)
        {
            var parserConfiguration = new Dictionary<string, object>();
            var networkProp = new NetworkProperties();
            if (model != null)
            {
                parserConfiguration.Add("script", model.RuleScript);
                parserConfiguration.Add("tls", model.EnableTLS);
                if (model.Delimited != null)
                {
                    parserConfiguration.Add("delimited", model.Delimited);
                }
                if (model.FixedLength != null)
                {
                    parserConfiguration.Add("fixedLength", model.FixedLength);
                }
                if (model.FixedLength != null)
                {
                    parserConfiguration.Add("maxMessageLength", model.MaxMessageLength);
                }
                if (model.EnableWebService != null)
                {
                    parserConfiguration.Add("enableWebService", model.EnableWebService);
                }
                if (model.IsMulticast != null)
                {
                    parserConfiguration.Add("isMulticast", model.IsMulticast);
                }
                if (model.EnableSwagger != null)
                {
                    parserConfiguration.Add("enableSwagger", model.EnableSwagger);
                }
                networkProp = new NetworkProperties
                {
                    Id = id.ToString(),
                    Host = model.Host,
                    Port = model.Port,
                    ParserType = model.ResolveMode == null ? PayloadParserType.Direct : (PayloadParserType)model.ResolveMode,
                    SSL = model.EnableSSL,
                    ParserConfiguration = parserConfiguration
                };
            }
            return networkProp;
        }
    }
}

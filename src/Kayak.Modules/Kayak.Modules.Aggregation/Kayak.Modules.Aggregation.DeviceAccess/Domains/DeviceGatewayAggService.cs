using Kayak.Core.Common;
using Kayak.Core.Common.Repsitories.Implementation;
using Kayak.Core.Common.Response;
using Kayak.IModuleServices.Aggregation.DeviceAccess;
using Kayak.IModuleServices.Aggregation.DeviceAccess.Models;
using Kayak.IModuleServices.DeviceAccess;
using Kayak.IModuleServices.DeviceAccess.Models;
using Kayak.IModuleServices.DeviceAccess.Queries;
using Kayak.IModuleServices.System;
using StackExchange.Redis;
using Surging.Core.CPlatform.Ioc;
using Surging.Core.DeviceGateway.Runtime.Core;
using Surging.Core.DeviceGateway.Runtime.Core.Implementation;
using Surging.Core.ProxyGenerator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Kayak.Modules.Aggregation.DeviceAccess.Domains
{
    public class DeviceGatewayAggService : ProxyServiceBase, IDeviceGatewayAggService, ISingleInstance
    {
        private readonly IDeviceGatewayManage _deviceGatewayManage;
        public DeviceGatewayAggService(IDeviceGatewayManage deviceGatewayManage) {
            _deviceGatewayManage = deviceGatewayManage;
        }

        public async Task<ApiResult<bool>> Open(List<int> ids)
        {
            var result = ApiResult<bool>.Failure(Core.Common.Enums.EnumReturnCode.FAIL);
            foreach (int id in ids) {
               var modelResult= await GetService<IDeviceGatewayService>().GetModelById(id);
                var model = modelResult.Result;
            IDeviceGatewayProvider deviceGatewayProvider = null;
                var transport = MessageTransport.Tcp;
                switch (model.GatewayTypeValue)
                {
                    case "TcpDeviceGateway":
                        deviceGatewayProvider = GetService<IDeviceGatewayProvider>(MessageTransport.Tcp.ToString());
                        break;
                    case "UdpDeviceGateway":
                        {
                            deviceGatewayProvider = GetService<IDeviceGatewayProvider>(MessageTransport.Tcp.ToString());
                            transport = MessageTransport.Udp;
                        }
                        break;
                    case "HttpDeviceGateway":
                        {
                            deviceGatewayProvider = GetService<IDeviceGatewayProvider>(MessageTransport.Tcp.ToString());
                            transport = MessageTransport.Http;
                        }
                        break;
                }
                if (deviceGatewayProvider != null)
                {
                    _deviceGatewayManage.CreateOrUpdate(deviceGatewayProvider, new DeviceGatewayProperties
                    {
                        ChannelId = model.NetWorkId.ToString(),
                        Description = model.Remark,
                        Id = model.Id.ToString(),
                        Name = model.Name,
                        Provider = model.GatewayTypeValue,
                        Protocol = model.ProtocolCode,
                        Transport = transport.ToString()
                    });
                    result= await GetService<IDeviceGatewayService>().Open(new List<int> { id });
                }
            }
            return result;
        }

        public async Task<ApiResult<bool>> Stop(List<int> ids)
        {
            var result = ApiResult<bool>.Failure(Core.Common.Enums.EnumReturnCode.FAIL);
            foreach (int id in ids)
            {
                var modelResult = await GetService<IDeviceGatewayService>().GetModelById(id);
                var model = modelResult.Result;
                IDeviceGatewayProvider deviceGatewayProvider = null;
                switch (model.GatewayTypeValue)
                {
                    case "TcpDeviceGateway":
                        deviceGatewayProvider = GetService<IDeviceGatewayProvider>(MessageTransport.Tcp.ToString());
                        break;
                    case "UdpDeviceGateway":
                            deviceGatewayProvider = GetService<IDeviceGatewayProvider>(MessageTransport.Tcp.ToString());
                        break;
                    case "HttpDeviceGateway":
                        deviceGatewayProvider = GetService<IDeviceGatewayProvider>(MessageTransport.Tcp.ToString());
                        break;
                }
                if (deviceGatewayProvider != null)
                {
                    _deviceGatewayManage.Shutdown(model.Id.ToString());
                    result = await GetService<IDeviceGatewayService>().Stop(new List<int> { id });
                }
            }
            return result;
        }

        public async Task<ApiResult<Page<DeviceGatewayModel>>> GetPageListAsync(DeviceGatewayQuery query)
        {
            var result = await GetService<IDeviceGatewayService>().GetPageAsync(query);
            return result;
        }

        public async Task<ApiResult<Page<DeviceGatewayAggModel>>> GetPageAsync(DeviceGatewayQuery query)
        {
            var apiResult = await GetService<IDeviceGatewayService>().GetPageAsync(query);
            var netWorkIds = apiResult.Result.Items.Select(p => p.NetWorkId).ToList();
            var gatewayTypeValues = apiResult.Result.Items.Select(p => p.GatewayTypeValue).ToList();

            var networkParts = await GetService<INetworkPartService>().GetListByIds(netWorkIds);
            var deviceGatewayTypes = await GetService<ISysDictionaryService>().GetSysDictionaryByParentCode(SysDictionaryCodes.DEVICEGATEWAYTYPE);
            var protocols = await GetService<IProtocolManageService>().GetProtocols();
            Page<DeviceGatewayAggModel> pageModel = new Page<DeviceGatewayAggModel>()
            {
                PageCount = apiResult.Result.PageCount,
                PageIndex = apiResult.Result.PageIndex,
                PageSize = apiResult.Result.PageSize,
                Total = apiResult.Result.Total,
                Items = apiResult.Result.Items.Select(x => new DeviceGatewayAggModel
                {
                    GatewayTypeValue = x.GatewayTypeValue,
                    Id = x.Id,
                    Name = x.Name,
                    NetWorkId = x.NetWorkId,
                    ProtocolCode = x.ProtocolCode,
                    Status = x.Status,
                    GatewayType = deviceGatewayTypes.Result.Where(p => p.Code == x.GatewayTypeValue).Select(p => new GatewayTypeDictionary
                    {
                        Code = p.Code,
                        Name = p.Name,
                        Value = p.Value,
                    }).FirstOrDefault(),
                    NetworkPart = networkParts.Result.Where(p => p.Id == x.NetWorkId).Select(p => new NetworkPartModel
                    {
                        Id = p.Id,
                        Name = p.Name,
                        ComponentTypeCode = p.ComponentTypeCode,
                         Host=p.Host,
                         Port=p.Port,
                          

                    }).FirstOrDefault(),
                    Protocol = protocols.Result.Where(p => p.ProtocolCode == x.ProtocolCode).Select(p => new ProtocolModel
                    {
                        Id = p.Id,
                        ProtocolName = p.ProtocolName,
                        ProtocolCode = p.ProtocolCode,
                         ConnProtocol = p.ConnProtocol

                    }).FirstOrDefault(),
                    Remark = x.Remark,
                    UpdateDate = x.UpdateDate,
                    CreateDate = x.CreateDate,

                }).ToList()
            };
            return ApiResult<Page<DeviceGatewayAggModel>>.Succeed(pageModel);
        }
    }
}

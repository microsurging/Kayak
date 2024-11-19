using Kayak.Core.Common.Response;
using Kayak.IModuleServices.Aggregation.DeviceManage;
using Kayak.IModuleServices.Aggregation.DeviceManage.Models;
using Kayak.IModuleServices.DeviceAccess;
using Kayak.IModuleServices.DeviceManage;
using Kayak.IModuleServices.DeviceManage.Models;
using Kayak.IModuleServices.DeviceManage.Queries;
using Surging.Core.CPlatform.Ioc;
using Surging.Core.CPlatform.Utilities;
using Surging.Core.DeviceGateway.Runtime.Core;
using Surging.Core.ProxyGenerator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kayak.Modules.Aggregation.DeviceManage.Domains
{
    public class DeviceAggService : ProxyServiceBase, IDeviceAggService, ISingleInstance
    { 
        public async Task<ApiResult<DeviceAggModel>> GetDeviceModel(int deviceId)
        {
            var list=await   GetService<IDeviceService>().GetDeviceByIds(new List<int> { deviceId });
            var device = list.Result.FirstOrDefault();
            var productCode= list.Result.Select(x => x.ProductCode).FirstOrDefault();
            var products=await GetService<IProductService>().GetProductByCondition(new ProductQuery { ProductCode= productCode??"" });
            var product=products.Result.FirstOrDefault();
            var deviceAccess = await GetService<IDeviceAccessService>().GetByProductCode(productCode??"");
            var gateway= await GetService<IDeviceGatewayService>().GetModelById(deviceAccess.Result.GatewayId);
            var protocol = await GetService<IProtocolManageService>().GetProtocolByCode(gateway.Result.ProtocolCode);
            var netWork = await GetService<INetworkPartService>().GetNetworkPartById(gateway.Result.NetWorkId);
            var ipaddress = $"{NetUtils.GetHostAddress(netWork.Result.Host)}:{netWork.Result.Port}";
            var deviceAggModel = new DeviceAggModel()
            {
                Id = device.Id,
                Name = device.Name,
                Code = device.Code,
                CreateDate = device.CreateDate,
                Remark = device.Remark,
                UpdateDate = device.UpdateDate,
                ConnProtocol = deviceAccess.Result.ConnProtocol,
                GatewayName = gateway.Result.Name,
                ProductCode = productCode ?? "",
                ProductName = product.ProductName,
                ProtocolName = protocol.Result.ProtocolName,
                 IpAddress= ipaddress
            }; 
            return ApiResult<DeviceAggModel>.Succeed(deviceAggModel);
        }


    }
}

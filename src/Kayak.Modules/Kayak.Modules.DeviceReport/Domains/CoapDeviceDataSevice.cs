using CoAP.Server.Resources;
using Kayak.IModuleServices.DeviceReport;
using Surging.Core.Protocol.Coap;
using Surging.Core.Protocol.Tcp.Runtime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kayak.Modules.DeviceReport.Domains
{
    public class CoapDeviceDataSevice : CoapBehavior, ICoapDeviceDataSevice
    {
        public CoapDeviceDataSevice() : base("DeviceData")
        {

        }

        protected override void DoGet(CoapExchange exchange)
        {
            exchange.Respond("hello,welcome"); 
        }

        // 重写 DoPost 方法来处理 POST 请求
        protected override void DoPost(CoapExchange exchange)
        {
            exchange.Respond("hello,welcome");
        }

        // 重写 DoPut 方法来处理 PUT 请求
        protected override void DoPut(CoapExchange exchange)
        {
        }

        // 重写 DoDelete 方法来处理 DELETE 请求
        protected override void DoDelete(CoapExchange exchange)
        {
        }
    }
}

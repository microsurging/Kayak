using Kayak.IModuleServices.DeviceReport;
using Surging.Core.Protocol.WS;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using WebSocketCore;

namespace Kayak.Modules.DeviceReport.Domains
{
    internal class WSDeviceDataService : WSBehavior, IWSDeviceDataService
    {
        protected override void OnMessage(MessageEventArgs e)
        {
            this.Client().SendTo($"send:{e.Data},\r\n reply:hello,welcome to you!",ID);
        }

        protected override void OnOpen()
        {
         
        }
    }
}

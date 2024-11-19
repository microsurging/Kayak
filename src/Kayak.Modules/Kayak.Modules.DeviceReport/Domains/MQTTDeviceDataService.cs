using Kayak.IModuleServices.DeviceReport;
using Kayak.IModuleServices.DeviceReport.Models;
using Surging.Core.Protocol.Mqtt.Internal.Messages;
using Surging.Core.Protocol.Mqtt.Internal.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kayak.Modules.DeviceReport.Domains
{
    public class MQTTDeviceDataService : MqttBehavior, IMQTTDeviceDataService
    {
        public override async Task<bool> Authorized(string username, string password)
        {
            bool result = false;
            if (username == "admin" && password == "123456")
                result = true;
            return await Task.FromResult(result);
        }

        public async Task<bool> IsOnline(string deviceId)
        {
            return await base.GetDeviceIsOnine(deviceId);
        }

        public async Task Publish(string deviceId, WillMessage message)
        {
            var willMessage = new MqttWillMessage
            {
                WillMessage = message.Message,
                Qos = message.Qos,
                Topic = message.Topic,
                WillRetain = message.WillRetain
            };
            await Publish(deviceId, willMessage);
            await RemotePublish(deviceId, willMessage);
        }
    }
}

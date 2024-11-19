using System;
using System.Collections.Generic;
using System.Linq;
using Surging.Core.CPlatform.Codecs.Message;
using Surging.Core.DeviceGateway.Runtime.Device;
using Surging.Core.DeviceGateway.Runtime.Device.MessageCodec;

namespace Kayak.Core.Codecs.Runtime
{
    public class DemoDeviceMessageCodec : DeviceMessageCodec
    {
        public override IObservable<IDeviceMessage> Decode(MessageDecodeContext context)
        {
            throw new NotImplementedException();
        }

        public override IObservable<EncodedMessage> Encode(MessageEncodeContext context)
        {
            throw new NotImplementedException();
        }
    }
}

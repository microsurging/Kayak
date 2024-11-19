
using Surging.Core.CPlatform.Codecs.Core;
using Surging.Core.CPlatform.Codecs.Message;
using Surging.Core.DeviceGateway.Runtime.Device;
using Surging.Core.DeviceGateway.Runtime.Device.MessageCodec;

namespace Kayak.Core.Codecs
{
    public class DemoTcpMessageCodec : DeviceMessageCodec
    {
        public override IObservable<IDeviceMessage> Decode(MessageDecodeContext context)
        {
            throw new NotImplementedException();
        }

        public override IObservable<IEncodedMessage> Encode(MessageEncodeContext context)
        {
            throw new NotImplementedException();
        }
    }
}

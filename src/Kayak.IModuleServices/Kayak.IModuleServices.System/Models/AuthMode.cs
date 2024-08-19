using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kayak.IModuleServices.System.Models
{
    public  enum AuthMode
    {
        Pwd=0,
        QRCode=1,
        PhoneCode=2,
        QQToken=3,
        WeChatToken=4
    }
}

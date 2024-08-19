using Kayak.IModuleServices.System.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kayak.IModuleServices.System.Queries
{
    public  class LoginParams
    {
        [JsonProperty("username")]
        public string UserName { get; set; }

        [JsonProperty("password")]
        public string Password { get; set; }

        [JsonProperty("email")]
        public string Email { get; set; }

        [JsonProperty("phone")]
        public string Phone { get; set; }

        [JsonProperty("authmode")]
        public AuthMode AuthMode { get; set; }

        [JsonProperty("qrcode")]
        public string QRCode { get; set; }

        [JsonProperty("qqtoken")]
        public string QQToken { get; set; }

        [JsonProperty("wechattoken")]
        public string WeChatToken { get; set; }

    }
}

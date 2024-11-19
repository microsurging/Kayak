using Kayak.IModuleServices.DeviceReport;
using Newtonsoft.Json;
using Surging.Core.Caching.Models;
using Surging.Core.CPlatform.Utilities;
using Surging.Core.Protocol.WS;
using Surging.Core.ProxyGenerator;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using WebSocketCore;

namespace Kayak.Modules.DeviceReport.Domains
{
    internal class ChatService : WSBehavior, IChatService
    {
        private static readonly ConcurrentDictionary<string, string> _users = new ConcurrentDictionary<string, string>();
        private static readonly ConcurrentDictionary<string, string> _clients = new ConcurrentDictionary<string, string>();

        protected override void OnOpen()
        {
           var _name = Context.QueryString["name"]; 
            if (!string.IsNullOrEmpty(_name))
            {
                _clients[ID] = _name;
                _users[_name] = ID;
            }
        }

        protected override void OnError( WebSocketCore.ErrorEventArgs e)
        {
          var msg = e.Message;
        }

        protected override void OnMessage(MessageEventArgs e)
        {
            if (_clients.ContainsKey(ID))
            {

                var message = JsonConvert.DeserializeObject<Dictionary<string, object>>(e.Data);
                //消息类型
               message.TryGetValue("type",out object @type);
                message.TryGetValue("toUser", out object toUser);
                message.TryGetValue("fromUser", out object fromUser);
                message.TryGetValue("msg", out object msg);
                message.TryGetValue("sdp", out object sdp);
                message.TryGetValue("iceCandidate", out object iceCandidate);
                

                Dictionary<String, Object> result = new Dictionary<String, Object>();
                result.Add("type", @type);

                //呼叫的用户不在线
                if (!_users.ContainsKey(toUser?.ToString()))
                {
                    result["type"]= "call_back";
                    result.Add("fromUser", "系统消息");
                    result.Add("msg", "Sorry，呼叫的用户不在线！");

                    this.Client().SendTo(JsonConvert.SerializeObject(result), ID);
                    return;
                }

                //对方挂断
                if ("hangup".Equals(@type))
                {
                    result.Add("fromUser", fromUser);
                    result.Add("msg", "对方挂断！");
                }

                //视频通话请求
                if ("call_start".Equals(@type))
                {
                    result.Add("fromUser", fromUser);
                    result.Add("msg", "1");
                }

                //视频通话请求回应
                if ("call_back".Equals(type))
                {
                    result.Add("fromUser", toUser);
                    result.Add("msg", msg);
                }

                //offer
                if ("offer".Equals(type))
                {
                    result.Add("fromUser", toUser); 
                    result.Add("sdp", sdp);
                }

                //answer
                if ("answer".Equals(type))
                {
                    result.Add("fromUser", toUser);
                    result.Add("sdp", sdp);
                }

                //ice
                if ("_ice".Equals(type))
                {
                    result.Add("fromUser", toUser);
                    result.Add("iceCandidate", iceCandidate);
                }

                this.Client().SendTo(JsonConvert.SerializeObject(result), _users.GetValueOrDefault(toUser?.ToString()));
            }
        }

        protected override void OnClose(CloseEventArgs e)
        {
           if( _clients.TryRemove(ID, out string name))
            _users.TryRemove (name, out string value);
        }

    }
} 
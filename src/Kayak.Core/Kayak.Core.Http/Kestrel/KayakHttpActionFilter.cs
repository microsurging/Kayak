using Surging.Core.CPlatform.Transport.Implementation;
using Surging.Core.CPlatform.Utilities;
using Surging.Core.KestrelHttpServer.Filters;
using Surging.Core.KestrelHttpServer.Filters.Implementation;
using Surging.Core.KestrelHttpServer.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kayak.Core.Http.Kestrel
{
    internal class KayakHttpActionFilter : IActionFilter
    { 
        public KayakHttpActionFilter()
        {
            
        }

        public Task OnActionExecuted(ActionExecutedContext filterContext)
        {
            return Task.CompletedTask;
        }

        public Task OnActionExecuting(ActionExecutingContext filterContext)
        {
            var headers= new Dictionary<string, string>();
            foreach(var header in filterContext.Context.Request.Headers)
            {
                headers.Add(header.Key.ToString(), header.Value.ToString());
            }
            RestContext.GetContext().SetAttachment("HttpHeaders", headers);
            //filterContext.Result = filterContext.Result;
            return Task.CompletedTask;
        }
    }
}

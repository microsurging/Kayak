using Kayak.Core.Common.Response;
using Kayak.IModuleServices.Aggregation.System;
using Kayak.IModuleServices.System;
using Kayak.IModuleServices.System.Models;
using Surging.Core.CPlatform.Ioc;
using Surging.Core.ProxyGenerator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kayak.Modules.Aggregation.System.Domains
{
    public class SysDicAggregationService : ProxyServiceBase, ISysDicAggregationService, ISingleInstance
    {
        public async Task<ApiResult<bool>> Add(SysDictionaryModel model)
        {
            return await GetService<ISysDictionaryService>().Add(model);
        }

        public async Task<ApiResult<bool>> DeleteById(string parentCode,List<int> ids)
        {
            return await GetService<ISysDictionaryService>().DeleteById(parentCode,ids);
        }

        public async Task<ApiResult<List<SysDictionaryModel>>> GetSysDictionaryByParentCode(string parentCode)
        {
            return await GetService<ISysDictionaryService>().GetSysDictionaryByParentCode(parentCode);
        }

        public async Task<ApiResult<bool>> Modify(SysDictionaryModel model)
        {
            return await GetService<ISysDictionaryService>().Modify(model);
        }
    }
}

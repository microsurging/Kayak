using Kayak.Core.Common;
using Kayak.Core.Common.Response;
using Kayak.IModuleServices.Aggregation.ServiceManage;
using Kayak.IModuleServices.Aggregation.ServiceManage.Models;
using Kayak.IModuleServices.ServiceManage;
using Kayak.IModuleServices.ServiceManage.Query;
using Kayak.IModuleServices.System;
using Surging.Core.CPlatform.Ioc;
using Surging.Core.ProxyGenerator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kayak.Modules.Aggregation.ServiceManage.Domains
{
    internal class RegCenterAggregationService : ProxyServiceBase, IRegCenterAggregationService, ISingleInstance
    {
        public async Task<ApiResult<Page<RegCenterAggregationModel>>> GetPageAsync(RegistryCenterQuery query)
        {
            var apiResult = await GetService<IRegistryCenterService>().GetPageAsync(query);
            var regCenterType = await GetService<ISysDictionaryService>().GetSysDictionaryByParentCode(SysDictionaryCodes.REGISTRYCENTERTYPE);
            Page<RegCenterAggregationModel> pageModel = new Page<RegCenterAggregationModel>()
            {
                PageCount = apiResult.Result.PageCount,
                PageIndex = apiResult.Result.PageIndex,
                PageSize = apiResult.Result.PageSize,
                Total = apiResult.Result.Total,
                Items = apiResult.Result.Items.Select(x => new RegCenterAggregationModel
                {
                    Name = x.Name,
                    Path = x.Path,
                    RegistryCenterType = x.RegistryCenterType,
                    CreateDate = x.CreateDate,
                    Host = x.Host,
                    Id = x.Id,
                    Port = x.Port,
                    RegCenterType = regCenterType.Result.Where(p => p.Value == x.RegistryCenterType).Select(p => new RegCenterDictionary
                    {
                        Code = p.Code,
                        Name = p.Name,
                        Value = p.Value,
                    }).FirstOrDefault(),
                    Remark = x.Remark,

                }).ToList()
            };
            return ApiResult<Page<RegCenterAggregationModel>>.Succeed(pageModel);
        }
    }
}

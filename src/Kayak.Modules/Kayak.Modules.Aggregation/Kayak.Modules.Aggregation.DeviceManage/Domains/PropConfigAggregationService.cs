using Kayak.Core.Common;
using Kayak.Core.Common.Response;
using Kayak.IModuleServices.Aggregation.DeviceManage;
using Kayak.IModuleServices.Aggregation.DeviceManage.Models;
using Kayak.IModuleServices.DeviceManage.Models;
using Kayak.IModuleServices.DeviceManage.Queries;
using Surging.Core.CPlatform.Ioc;
using Surging.Core.ProxyGenerator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kayak.Modules.Aggregation.DeviceManage.Domains
{
    internal class PropConfigAggregationService : ProxyServiceBase, IPropConfigAggregationService, ISingleInstance
    {
        public async Task<ApiResult<Page<PropConfigModel>>> GetPageAsync(PropertyConfigureQuery query)
        {

            var apiResult = await GetService<IModuleServices.DeviceManage.IPropertyConfigureService>().GetPageAsync(query); 
            var datatypes = await GetService<IModuleServices.System.ISysDictionaryService>().GetSysDictionaryByParentCode(SysDictionaryCodes.DATATYPE); 
            Page<PropConfigModel> pageModel = new Page<PropConfigModel>()
            {
                PageCount = apiResult.Result.PageCount,
                PageIndex = apiResult.Result.PageIndex,
                PageSize = apiResult.Result.PageSize,
                Total = apiResult.Result.Total,
                Items = apiResult.Result.Items.Select(x => new PropConfigModel
                {
                    PropertyName = x.PropertyName, 
                     Id = x.Id,
                      MaxLength = x.MaxLength,
                    PropertyId = x.PropertyId,
                    CreateDate = x.CreateDate,
                    Remark = x.Remark,
                    ReadWriteType = x.ReadWriteType,
                    Step = x.Step,
                    CorrelationFrom = x.CorrelationFrom,
                    CorrelationId = x.CorrelationId,
                    DataTypeValue = x.DataTypeValue,
                    DefaultValue = x.DefaultValue,
                    UnitValue = x.UnitValue,
                    Precision = x.Precision,
                    SourceValue = x.SourceValue,
                    ValueRange = x.ValueRange,
                    UpdateDate = x.UpdateDate,
                      PropDataType  = datatypes.Result.Where(p => p.Code == x.DataTypeValue).Select(p => new DataTypeDictionary
                      {
                        Code = p.Code,
                        Name = p.Name,
                        Value = p.Value,
                    }).FirstOrDefault(), 


                }).ToList()
            };
            return ApiResult<Page<PropConfigModel>>.Succeed(pageModel);
        }
    }
}

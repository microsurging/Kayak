using Kayak.Core.Common.Repsitories.Implementation;
using Kayak.Core.Common.Response;
using Kayak.DataAccess.DeviceData.Entities;
using Kayak.IModuleServices.DeviceManage;
using Kayak.IModuleServices.DeviceManage.Models;
using Kayak.IModuleServices.DeviceManage.Queries;
using Kayak.Modules.DeviceManage.Repositories;
using Surging.Core.CPlatform.Ioc;
using Surging.Core.ProxyGenerator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kayak.Modules.DeviceManage.Domains
{
    public class PropertyThresholdService : ProxyServiceBase, IPropertyThresholdService, ISingleInstance
    {
        private readonly PropertyThresholdRepository _repository;

        public PropertyThresholdService(PropertyThresholdRepository repository) {
            _repository = repository;
        }

        public async Task<ApiResult<bool>> Edit(string key, List<PropertyThresholdModel> list)
        {  
            var propertyId= list.Select(p=>p.PropertyId).FirstOrDefault();
            var propertyThresholds = await _repository.GetByPropertyId(propertyId);     
            var ids = list.Select(p => p.Id).ToList();
            var addList = list.Where(p => p.Id == null).ToList();
            var modifyList = list.Where(p => p.Id != null).ToList();
            await _repository.AddList(addList);
            foreach (var item in modifyList)
                await _repository.Modify(item);
            var delList = propertyThresholds.Where(p => !ids.Contains(p.Id));
            await _repository.DelBatch(delList.ToList());
            return ApiResult<bool>.Succeed(true);
        }
 

        public async Task<ApiResult<List<PropertyThresholdModel>>> Get(PropertyThresholdQuery query)
        {
            var result = await _repository.Get(query);
            return ApiResult<List<PropertyThresholdModel>>.Succeed(result);
        }

        public async Task<ApiResult<List<PropertyThresholdModel>>> GetByPropertyId(int propertyId)
        {
            var result = await _repository.GetByPropertyId(propertyId);
            return ApiResult<List<PropertyThresholdModel>>.Succeed(result);
        }
    }
}

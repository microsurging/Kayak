using Kayak.Core.Common.Response;
using Kayak.IModuleServices.DeviceManage.Models;
using Kayak.IModuleServices.DeviceManage.Queries;
using Kayak.IModuleServices.DeviceManage;
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
    public class FunctionParameterService : ProxyServiceBase, IFunctionParameterService, ISingleInstance
    {
        private readonly FunctionParameterRepository _repository;

        public FunctionParameterService(FunctionParameterRepository repository)
        {
            _repository = repository;
        }

        public async Task<ApiResult<bool>> Edit(string key, List<FunctionParameterModel> list)
        {
            var temp = list.Select(p => new { p.FunctionId,p.ParameterType }).FirstOrDefault();
            var propertyThresholds = await _repository.GetByFunctionId(temp.FunctionId,temp.ParameterType);
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

        public async Task<ApiResult<List<FunctionParameterModel>>> Get(FunctionParameterQuery query)
        {
            var result = await _repository.Get(query);
            return ApiResult<List<FunctionParameterModel>>.Succeed(result);
        }

        public async Task<ApiResult<List<FunctionParameterModel>>> GetByFunctionId(int functionId, string parameterType)
        {
            var result = await _repository.GetByFunctionId(functionId, parameterType);
            return ApiResult<List<FunctionParameterModel>>.Succeed(result);
        }
    }
}

using Kayak.Core.Common.Enums;
using Kayak.Core.Common.Extensions;
using Kayak.Core.Common.Intercepte.Model;
using Kayak.Core.Common.Response;
using Kayak.IModuleServices.System;
using Kayak.IModuleServices.System.Models;
using Kayak.IModuleServices.System.Queries;
using Kayak.Modules.System.Repositories;
using Surging.Core.CPlatform.Ioc;
using Surging.Core.ProxyGenerator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kayak.Modules.System.Domians
{
    public class SysDataTypeService : ProxyServiceBase, ISingleInstance, ISysDataTypeService
    {
        private readonly SysDataTypeRepository _repository;
        public SysDataTypeService(SysDataTypeRepository repository)
        {
            _repository = repository;
        }
        public async Task<ApiResult<bool>> Add(SysDataTypeModel model)
        {
            var result = await _repository.Add(model);
            return ApiResult<bool>.Succeed(result);
        }

        public async Task<ApiResult<bool>> DeleteById(List<int> ids)
        {
            var result = await _repository.DeleteById(ids);
            return ApiResult<bool>.Succeed(result);
        }

        public async Task<ApiResult<Page<SysDataTypeModel>>> GetPageAsync(SysDataTypeQuery query)
        {
            var result = await _repository.GetPageAsync(query);
            return ApiResult<Page<SysDataTypeModel>>.Succeed(result);
        }
         
        public async Task<ApiResult<List<SysDataTypeModel>>> GetList()
        {
            var result = await _repository.GetList();
            return ApiResult<List<SysDataTypeModel>>.Succeed(result);
        }

        public async Task<ApiResult<bool>> Modify(SysDataTypeModel model)
        {
            var result = await _repository.Modify(model);
            return ApiResult<bool>.Succeed(result);
        }

        public async Task<ApiResult<bool>> Validate(SysDataTypeModel model)
        {
            var message = "";
            if (!model.Name.IsNullOrEmpty())
            {
                var result = await _repository.ExistsModelByName(model.Id, model.Name);
                message = result ? "单位名称已存在" : message;
            }
            if (model.Value != null)
            {
                var result = await _repository.ExistsModelByValue(model.Id,model.Value);
                message = result ? "单位数值已存在" : message;
            }
            return message.IsNullOrEmpty() ? ApiResult<bool>.Succeed(true) : ApiResult<bool>.Failure(EnumReturnCode.CUSTOM_ERROR, message);
        }
    }
}

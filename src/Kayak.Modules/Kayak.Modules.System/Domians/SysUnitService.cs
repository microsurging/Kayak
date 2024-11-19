using Kayak.Core.Common.Enums;
using Kayak.Core.Common.Extensions;
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
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Kayak.Modules.System.Domians
{
    public class SysUnitService : ProxyServiceBase, ISingleInstance, ISysUnitService
    {
        private readonly SysUnitRepository _repository;
        public SysUnitService(SysUnitRepository repository)
        {
            _repository = repository;
        }
        public async Task<ApiResult<bool>> Add(SysUnitModel model)
        {
            var result = await _repository.Add(model);
            return ApiResult<bool>.Succeed(result);
        }

        public async Task<ApiResult<bool>> DeleteById(List<int> ids)
        {
            var result = await _repository.DeleteById(ids);
            return ApiResult<bool>.Succeed(result);
        }

        public async Task<ApiResult<List<SysUnitModel>>> GetList()
        {
            var result = await _repository.GetList();
            return ApiResult<List<SysUnitModel>>.Succeed(result);
        }

        public async Task<ApiResult<Page<SysUnitModel>>> GetPageAsync(SysUnitQuery query)
        {
            var result = await _repository.GetPageAsync(query);
            return ApiResult<Page<SysUnitModel>>.Succeed(result);
        }

        public async Task<ApiResult<bool>> Modify(SysUnitModel model)
        {
            var result = await _repository.Modify(model);
            return ApiResult<bool>.Succeed(result);
        }

        public async Task<ApiResult<bool>> Validate(SysUnitModel model)
        {
            var message = "";
            if (!model.Name.IsNullOrEmpty())
            {
                var result = await _repository.ExistsModelByName(model.Id,model.Name);
                message = result ? "单位名称已存在" : message;
            }
            if (model.Value !=null)
            {
                var result = await _repository.ExistsModelByValue(model.Id,model.Value);
                message = result ? "单位数值已存在" : message;
            }
            return message.IsNullOrEmpty() ? ApiResult<bool>.Succeed(true) : ApiResult<bool>.Failure(EnumReturnCode.CUSTOM_ERROR, message);
        }
    }
}

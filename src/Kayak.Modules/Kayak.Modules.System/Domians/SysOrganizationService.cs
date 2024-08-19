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
    public class SysOrganizationService : ProxyServiceBase, ISingleInstance, ISysOrganizationService
    {
        private readonly SysOrganizationRepository _repository;
        public SysOrganizationService(SysOrganizationRepository repository) {
            _repository = repository;
        }

        public async Task<ApiResult<bool>> Add(SysOrganizationModel model)
        {
            var result = new ApiResult<bool>();
            result.Result = await _repository.Add(model);
            return result;
        }

        public async Task<ApiResult<bool>> DeleteById(List<int> ids)
        {
            var result = await _repository.DeleteById(ids);
            return ApiResult<bool>.Succeed(result);
        }

        public async Task<ApiResult<Page<SysOrganizationModel>>> GetPageAsync(SysOrganizationQuery query)
        {
            var result = await _repository.GetPageAsync(query);
            return ApiResult<Page<SysOrganizationModel>>.Succeed(result);
        }

        public async Task<ApiResult<SysOrganizationModel>> GetSysOrganization(int id)
        {
            var result = new ApiResult<SysOrganizationModel>();
            result.Result = await _repository.GetSysOrganization(id);
            return result;
        }

        public async Task<ApiResult<List<SysOrganizationModel>>> GetSysOrganizationByCondition(SysOrganizationQuery query)
        {
            var result = await _repository.GetSysOrganizationByCondition(query);
            return ApiResult<List<SysOrganizationModel>>.Succeed(result);
        }

        public async Task<ApiResult<bool>> Modify(SysOrganizationModel model)
        {
            var result = await _repository.Modify(model);
            return ApiResult<bool>.Succeed(result);
        }

        public async Task<ApiResult<bool>> Validate(SysOrganizationModel model)
        {
            var message = "";
            if (!model.Name.IsNullOrEmpty())
            {
                var result = await _repository.ExistsModelByName(model.Name);
                message = result ? "机构名称已存在" : message;
            }
            
            return message.IsNullOrEmpty() ? ApiResult<bool>.Succeed(true) : ApiResult<bool>.Failure(EnumReturnCode.CUSTOM_ERROR, message);
        }
    }
}

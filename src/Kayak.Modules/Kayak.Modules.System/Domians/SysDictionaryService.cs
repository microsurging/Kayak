using Kayak.Core.Common.Enums;
using Kayak.Core.Common.Extensions;
using Kayak.Core.Common.Repsitories.Implementation;
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
    public class SysDictionaryService:ProxyServiceBase, ISingleInstance, ISysDictionaryService
    {

        private readonly SysDictionaryRepository _sysDictionaryRepository;
        public SysDictionaryService(SysDictionaryRepository sysDictionaryRepository) {

            _sysDictionaryRepository = sysDictionaryRepository;
        }

        public async Task<ApiResult<bool>> Add(SysDictionaryModel model)
        { 
            var result = await _sysDictionaryRepository.Add(model);
            return ApiResult<bool>.Succeed(result);
        }

        public async Task<ApiResult<Page<SysDictionaryModel>>> GetPageAsync(SysDictionaryQuery query)
        {
            var result = await _sysDictionaryRepository.GetPageAsync(query);
            return ApiResult<Page<SysDictionaryModel>>.Succeed(result);
        }

        public async Task<ApiResult<List<SysDictionaryModel>>> GetSysDictionaryByCondition(SysDictionaryQuery query)
        {
            var result = await _sysDictionaryRepository.GetSysDictionaryByCondition(query);
            return ApiResult<List<SysDictionaryModel>>.Succeed(result);
        }

        public async Task<ApiResult<SysDictionaryModel>> GetSysDictionary(string code)
        { 
            var result = await _sysDictionaryRepository.GetSysDictionary(code);
            return ApiResult<SysDictionaryModel>.Succeed(result);
        }

        public async  Task<ApiResult<bool>> DeleteById(string parentCode, List<int> ids)
        {
            var result = await _sysDictionaryRepository.DeleteById(ids);
            return ApiResult<bool>.Succeed(result);
        }

        public async Task<ApiResult<bool>> Stop(List<int> ids)
        {
            var result = await _sysDictionaryRepository.Stop(ids);
            return ApiResult<bool>.Succeed(result);
        }

        public async Task<ApiResult<bool>> Open(List<int> ids)
        {
            var result = await _sysDictionaryRepository.Open(ids);
            return ApiResult<bool>.Succeed(result);
        }

        public async Task<ApiResult<bool>> Modify(SysDictionaryModel model)
        {
            var result = await _sysDictionaryRepository.Modify(model);
            return ApiResult<bool>.Succeed(result);
        }

        public async Task<ApiResult<bool>> Validate(SysDictionaryModel model)
        {
            var message = "";
            if (!model.Name.IsNullOrEmpty())
            {
                var result = await _sysDictionaryRepository.ExistsModelByName(model.Id, model.ParentCode ?? "", model.Name);
                message = result ? "名称已存在" : message;
            }
            if (model.Code != null)
            {
                var result = await _sysDictionaryRepository.ExistsModelByCode(model.Id,model.ParentCode ?? "", model.Code);
                message = result ? "字典编码已存在" : message;
            }
            if (model.Value != null)
            {
                var result = await _sysDictionaryRepository.ExistsModelByValue(model.ParentCode??"", model.Value);
                message = result ? "字典数值已存在" : message;
            }
            return message.IsNullOrEmpty() ? ApiResult<bool>.Succeed(true) : ApiResult<bool>.Failure(EnumReturnCode.CUSTOM_ERROR, message);
        }

        public async Task<ApiResult<List<SysDictionaryModel>>> GetSysDictionaryByParentCode(string parentCode)
        {
            var result = await _sysDictionaryRepository.GetSysDictionaryByParentCode(parentCode);
            return ApiResult<List<SysDictionaryModel>>.Succeed(result);
        }
    }
}

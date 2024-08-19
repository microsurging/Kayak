using Kayak.Core.Common.Response;
using Kayak.IModuleServices.ServiceManage;
using Kayak.IModuleServices.ServiceManage.Model;
using Kayak.IModuleServices.ServiceManage.Query;
using Kayak.Modules.ServiceManage.Repositories;
using Surging.Core.CPlatform.Ioc;
using Surging.Core.ProxyGenerator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Kayak.Modules.ServiceManage.Domains
{
    public class RegistryCenterService : ProxyServiceBase, IRegistryCenterService, ISingleInstance
    {
        private readonly RegistryCenterRepository _repository;
        public RegistryCenterService(RegistryCenterRepository repository) {
            _repository = repository;
        }
        public async Task<ApiResult<bool>> Add(RegistryCenterModel model)
        {
            var result = await _repository.Add(model);
            return ApiResult<bool>.Succeed(result);
        }

        public async Task<ApiResult<bool>> DeleteById(List<int> ids)
        {
            var result = await _repository.DeleteById(ids);
            return ApiResult<bool>.Succeed(result);
        }

        public async Task<ApiResult<List<RegistryCenterModel>>> GetList()
        {
            var result = await _repository.GetList();
            return ApiResult<List<RegistryCenterModel>>.Succeed(result);
        }

        public async Task<ApiResult<Page<RegistryCenterModel>>> GetPageAsync(RegistryCenterQuery query)
        {
            var result = await _repository.GetPageAsync(query);
            return ApiResult<Page<RegistryCenterModel>>.Succeed(result);
        }

        public async Task<ApiResult<bool>> Modify(RegistryCenterModel model)
        {
            var result = await _repository.Modify(model);
            return ApiResult<bool>.Succeed(result);
        }
    }
}

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
    internal class ModuleService : ProxyServiceBase, IModuleService, ISingleInstance
    {
        private readonly ModuleRepository _repository;

        public ModuleService(ModuleRepository repository)
        {
            _repository = repository;
        }
        public async Task<ApiResult<bool>> Add(ModuleModel model)
        {
            var result = await _repository.Add(model);
            return ApiResult<bool>.Succeed(result);
        }

        public async Task<ApiResult<Page<ModuleModel>>> GetPageAsync(ModuleQuery query)
        {
            var result = await _repository.GetPageAsync(query);
            return ApiResult<Page<ModuleModel>>.Succeed(result);
        }

        public async Task<ApiResult<bool>> Modify(ModuleModel model)
        {
            var result = await _repository.Modify(model);
            return ApiResult<bool>.Succeed(result);
        }

    }
}

using Kayak.Core.Common.Repsitories.Implementation;
using Kayak.Core.Common.Response;
using Kayak.IModuleServices.ServiceManage;
using Kayak.IModuleServices.ServiceManage.Model;
using Kayak.IModuleServices.ServiceManage.Query;
using Kayak.Modules.ServiceManage.Repositories;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
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
    public class BlackWhiteListService : ProxyServiceBase, IBlackWhiteListService, ISingleInstance
    {
        private readonly BlackWhiteListRepository _repository;
        public BlackWhiteListService(BlackWhiteListRepository repository)
        {
            _repository = repository;
        }
        public async Task<ApiResult<bool>> Add(BlackWhiteListModel model)
        {
            var result = await _repository.Add(model);
            return ApiResult<bool>.Succeed(result);
        }

        public async Task<ApiResult<bool>> DeleteById(List<int> ids)
        {
            var result = await _repository.DeleteById(ids);
            return ApiResult<bool>.Succeed(result);
        }

        public async Task<ApiResult<bool>> Disable(List<int> ids)
        {
            var result = await _repository.Disable(ids);
            return ApiResult<bool>.Succeed(result);
        }

        public async Task<ApiResult<bool>> Enable(List<int> ids)
        {
            var result = await _repository.Enable(ids);
            return ApiResult<bool>.Succeed(result);
        }

        public async Task<ApiResult<List<BlackWhiteListModel>>> GetListByIds(List<int> ids)
        {
            var result = await _repository.GetListByIds(ids);
            return ApiResult<List<BlackWhiteListModel>>.Succeed(result);
        }

        public async Task<ApiResult<Page<BlackWhiteListModel>>> GetPageAsync(BlackWhiteListQuery query)
        {
            var result = await _repository.GetPageAsync(query);
            return ApiResult<Page<BlackWhiteListModel>>.Succeed(result);
        }

        public async Task<ApiResult<bool>> Modify(BlackWhiteListModel model)
        {
            var result = await _repository.Modify(model);
            return ApiResult<bool>.Succeed(result);
        }
    }
}

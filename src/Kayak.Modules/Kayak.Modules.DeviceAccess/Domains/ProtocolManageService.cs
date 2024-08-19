using Kayak.Core.Common.Repsitories.Implementation;
using Kayak.Core.Common.Response;
using Kayak.IModuleServices.DeviceAccess;
using Kayak.IModuleServices.DeviceAccess.Models;
using Kayak.IModuleServices.DeviceAccess.Queries;
using Kayak.Modules.DeviceAccess.Repositories;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Surging.Core.CPlatform.Ioc;
using Surging.Core.ProxyGenerator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Kayak.Modules.DeviceAccess.Domains
{
    public class ProtocolManageService : ProxyServiceBase, ISingleInstance,IProtocolManageService
    {
        private readonly ProtocolManageRepository _repository;
        public ProtocolManageService(ProtocolManageRepository repository) {
            _repository = repository;
        }
        public async Task<ApiResult<bool>> Add(ProtocolModel model)
        {
             var result= await _repository.Add(model);
            return  ApiResult<bool>.Succeed(result);
        }

        public async  Task<ApiResult<bool>> CancelPublish(List<int> ids)
        {
            var result = await _repository.CancelPublish(ids);
            return ApiResult<bool>.Succeed(result);
        }

        public async Task<ApiResult<bool>> DeleteById(List<int> ids)
        {
            var result = await _repository.DeleteById(ids);
            return ApiResult<bool>.Succeed(result);
        }

        public async Task<ApiResult<Page<ProtocolModel>>> GetPageAsync(ProtocolQuery query)
        {
            var result=await _repository.GetPageAsync(query);
            return ApiResult<Page<ProtocolModel>>.Succeed(result);
        }

        public async Task<ApiResult<ProtocolModel>> GetProtocol(int id)
        {
            var result = await _repository.GetProtocol(id);
            return ApiResult<ProtocolModel>.Succeed(result);
        }

        public async Task<ApiResult<bool>> Republish(List<int> ids)
        {
            var t =new  int[]{ };
            var result = await _repository.Republish(ids);
            return ApiResult<bool>.Succeed(result);
        }
    }
}

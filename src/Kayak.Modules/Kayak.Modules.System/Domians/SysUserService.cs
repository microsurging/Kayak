using Kayak.Core.Common.Enums;
using Kayak.Core.Common.Extensions;
using Kayak.Core.Common.Response;
using Kayak.IModuleServices.System;
using Kayak.IModuleServices.System.Models;
using Kayak.IModuleServices.System.Queries;
using Kayak.Modules.System.Repositories;
using Surging.Core.CPlatform.Ioc;
using Surging.Core.CPlatform.Transport.Implementation;
using Surging.Core.ProxyGenerator;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Kayak.Modules.System.Domians
{
    public class SysUserService : ProxyServiceBase, ISingleInstance, ISysUserService
    {
        private readonly SysUserRepository _repository;
        private readonly PayloadContext _payloadContext;
        public SysUserService(SysUserRepository repository, PayloadContext payloadContext)
        {
            _repository = repository;
            _payloadContext = payloadContext;
        }
        public Task<bool> Add(SysUserModel model)
        {
            return _repository.Add(model);
        }

        public async Task<IdentityInfo> Authentication(LoginParams loginParams)
        {
            var result = new IdentityInfo();
            switch (loginParams.AuthMode)
            {
                case AuthMode.Pwd:
                    {
                        result = await AuthenticationByPwd(loginParams);
                        break;
                    }
            }
            return result;
        }

        public async Task<IdentityInfo> AuthenticationByPwd(LoginParams loginParams)
        {
            var userModel = await _repository.GetUserModelByAccount(new SysUserModel
            {
                UserName = loginParams.UserName,
                Email = loginParams.Email,
                Phone = loginParams.Phone
            });
            var result = userModel == null ? default : new IdentityInfo() { UserId = userModel.UserId, UserName = userModel.UserName };

            if (userModel != null && userModel.Status != 1)
            {
                result = null;
            }
            else if (userModel != null && userModel.Password != userModel.Password)
            {
                result = null;
            }
            return result;
        }

        public async Task<ApiResult<bool>> ChangeDisable(List<int> ids)
        {
            var result = await _repository.ChangeDisable(ids);
            return ApiResult<bool>.Succeed(result);
        }

        public async Task<ApiResult<bool>> ChangeEnable(List<int> ids)
        {
            var result = await _repository.ChangeEnable(ids);
            return ApiResult<bool>.Succeed(result);
        }

        public async  Task<ApiResult<bool>> DeleteById(List<int> ids)
        {
            var result = await _repository.DeleteById(ids);
            return ApiResult<bool>.Succeed(result);
        }

        public async Task<ApiResult<Page<SysUserModel>>> GetPageAsync(SysUserQuery query)
        {
            var result = await _repository.GetPageAsync(query);
            return ApiResult<Page<SysUserModel>>.Succeed(result);
        }

        public async Task<ApiResult<UserInfo>> GetUserInfo()
        {
            var userId = _payloadContext.Get<IdentityInfo>().UserId;
            var result = await _repository.GetUserInfoById(userId);
            return ApiResult<UserInfo>.Succeed(result);
        }

        public async  Task<ApiResult<bool>> Modify(SysUserModel model)
        {
            var result = await _repository.Modify(model);
            return ApiResult<bool>.Succeed(result);
        }

        public async Task<ApiResult<bool>> Validate(SysUserModel model)
        {
            var message = "";
           if(!model.UserName.IsNullOrEmpty())
            {
                var result = await _repository.ExistsModelByName(model.UserId,model.UserName);
                message = result ? "用户名已存在" : message;
            }
            if (model.Email!=null)
            {
                var result = await _repository.ExistsModelByEmail(model.UserId, model.Email);
                message = result ? "邮箱已存在" : message;
            }
            if (model.Phone != null)
            {
                var result = await _repository.ExistsModelByPhone(model.UserId, model.Phone);
                message = result ? "手机号已存在" : message;
            }
            return message.IsNullOrEmpty() ? ApiResult<bool>.Succeed(true) : ApiResult<bool>.Failure(EnumReturnCode.CUSTOM_ERROR, message);
        }
    }
}

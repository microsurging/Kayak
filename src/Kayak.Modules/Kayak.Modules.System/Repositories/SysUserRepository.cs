
using Kayak.Core.Common.Repsitories;
using Kayak.Core.Common.Repsitories.Implementation;
using Kayak.Core.Common.Response;
using Kayak.IModuleServices.System.Models;
using Kayak.IModuleServices.System.Queries;
using Microsoft.EntityFrameworkCore;
using Kayak.Core.Common.Extensions;
using Kayak.Core.Common.Enums;
using Surging.Core.CPlatform.Ioc;
using Kayak.DataAccess.DeviceData;
using Kayak.DataAccess.DeviceData.Entities;
using Surging.Core.CPlatform.Utilities;
using Newtonsoft.Json.Linq;
using System.Numerics;

namespace Kayak.Modules.System.Repositories
{
    public class SysUserRepository:BaseRepository, ISingleInstance
    {
        private readonly DataContext _dataContext;
        private readonly IEFRepository<SysUser> _repository;
        public SysUserRepository(IEFRepository<SysUser> repository)
        {
            _dataContext = ServiceLocator.GetService<DataContext>(AppConfig.DeviceDataOptions.DatabaseType.ToString());
            _repository = repository;
        }

        public async Task<bool> Add(SysUserModel model)
        {
            model.UpdateTime = DateTime.Now;
            model.CreateTime = DateTime.Now;
            _dataContext.SysUser.Add(ToEntity(model));
            var i = await _dataContext.SaveChangesAsync() > 0;
            return i;
        }

        public async Task<UserInfo>  GetUserInfoById(int userId)
        {
            var entity = await _dataContext.SysUser.AsNoTracking().Where(p => p.Id == userId).FirstOrDefaultAsync();
            return ToUserInfo(entity);
        }

        public async Task<bool> ExistsModelByEmail(int userId, string email)
        {
            return await _dataContext.SysUser.AsNoTracking().AnyAsync(p => p.Email == email && p.Id != userId);
        }

        public async Task<bool> ExistsModelByPhone(int userId, string phone)
        {
            return await _dataContext.SysUser.AsNoTracking().AnyAsync(p => p.Phone == phone && p.Id != userId);
        }

        public async Task<bool> ExistsModelByName(int userId,string username)
        {
            return await _dataContext.SysUser.AsNoTracking().AnyAsync(p => p.UserName == username && p.Id!=userId);
        }

        public async Task<SysUserModel> GetUserModelByAccount(SysUserModel model)
        {
            var entity = await _dataContext.SysUser.AsNoTracking().Where(p => p.UserName == model.UserName || p.Email == model.Email || p.Phone == model.Phone).FirstOrDefaultAsync();
            return ToModel(entity);
        }

        public Task<bool> DeleteById(List<int> ids)
        {
            var result = _repository.Instance(_dataContext).ModifyBy(new SysUser { IsDeleted = true }, p => ids.Contains(p.Id), "IsDeleted") > 0;
            return Task.FromResult(result);
        }

        public Task<bool> ChangeDisable(List<int> ids)
        {
            var result = _repository.Instance(_dataContext).ModifyBy(new SysUser { Status = 0 }, p => ids.Contains(p.Id), "Status") > 0;
            return Task.FromResult(result);
        }

        public Task<bool> Modify(SysUserModel model)
        {
            var entity = ToEntity(model);
            var result = _repository.Instance(_dataContext).ModifyBy(entity,p=>p.Id== model.UserId,"Email", "Phone", "Phone", "Password", "RealName", "Sex", "PhoneNumber", "Remark", "UpdateDate") > 0;
            return Task.FromResult(result);
        }

        public Task<bool> ChangeEnable(List<int> ids)
        {
            var result = _repository.Instance(_dataContext).ModifyBy(new SysUser { Status = 1 }, p => ids.Contains(p.Id), "Status") > 0;
            return Task.FromResult(result);
        }

        public async Task<Page<SysUserModel>> GetPageAsync(SysUserQuery query)
        {
            var result = new Page<SysUserModel>();
            var entities = await _repository.Instance(_dataContext).GetPageList(query.Page, query.PageSize, x => x
            .WhereIF(query.Id.HasValue, e => e.Id == query.Id)
            .WhereIF(!query.Ids.IsNullOrEmpty(), e => query.Ids.Contains(e.Id))
            .WhereIF(!query.Phone.IsNullOrEmpty(), e => e.Phone == query.Phone)
            .WhereIF(!query.Email.IsNullOrEmpty(), e => e.Email == query.Email)
            .WhereIF(!query.UserName.IsNullOrEmpty(), e => e.UserName == query.UserName)
             .WhereIF(query.Sex!=null, e => e.Sex == (int)query.Sex)
            .Where(e => e.IsDeleted == false)
            .OrderByDescending(e => e.Id)
            );
            result.Items = entities.Items.Select(p => ToModel(p)).ToList();
            result.Total = entities?.Total ?? 0;
            result.PageCount = entities?.PageCount ?? 0;
            result.PageIndex = entities?.PageIndex ?? 0;
            result.PageSize = entities?.PageSize ?? 0;
            return result;
        }


        public SysUser ToEntity(SysUserModel model)
        {
            return new SysUser
            {
                CreateDate = DateTime.Now,
                Email = model.Email,
                Phone = model.Phone,
                Password = model.Password,
                Status = model.Status,
                Avatar = model.Avatar,
                RealName = model.RealName,
                Sex = (int)model.Sex,
                PhoneNumber = model.PhoneNumber,
                UpdateDate = DateTime.Now,
                QQToken = model.QQToken,
                 Remark=model.Remark,
                
                WeChatToken = model.WeChatToken,
                UserName = model.UserName,

            };
        }

        public  UserInfo ToUserInfo(SysUser entity)
        {
            return entity == null ? default : new UserInfo
            {
                UserId = entity.Id,  
                CreateDate = DateTime.Now,
                Email = entity.Email,
                Phone = entity.Phone,
                Avatar = entity.Avatar,
                RealName = entity.RealName,
                Sex = Enum.Parse<UserGenderEnum>(entity.Sex.ToString()),
                PhoneNumber = entity.PhoneNumber,  
                UserName = entity.UserName,
            };
        }

        public SysUserModel ToModel(SysUser entity)
        {

            return entity==null?default:new SysUserModel
            {
                 UserId=entity.Id,
                CreateTime = entity.CreateDate,
                Password = entity.Password,
                Status = entity.Status,
                Email = entity.Email,
                Phone = entity.Phone,
                 Avatar = entity.Avatar,
                  RealName = entity.RealName,
                 Sex=Enum.Parse<UserGenderEnum>(entity.Sex.ToString()),
                PhoneNumber = entity.PhoneNumber,
                UpdateTime = entity.UpdateDate,
                QQToken = entity.QQToken,
                WeChatToken = entity.WeChatToken,
                UserName = entity.UserName,
            };

        }
    }
}

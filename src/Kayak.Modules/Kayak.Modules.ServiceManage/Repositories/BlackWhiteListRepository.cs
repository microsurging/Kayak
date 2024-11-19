using Kayak.Core.Common.Context;
using Kayak.Core.Common.Extensions;
using Kayak.Core.Common.Repsitories;
using Kayak.Core.Common.Response;
using Kayak.DataAccess.DeviceData;
using Kayak.DataAccess.DeviceData.Entities;
using Kayak.IModuleServices.ServiceManage.Model;
using Kayak.IModuleServices.ServiceManage.Query;
using Microsoft.EntityFrameworkCore;
using Surging.Core.CPlatform.Ioc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kayak.Modules.ServiceManage.Repositories
{
    public class BlackWhiteListRepository : BaseRepository, ISingleInstance
    {
        private readonly IEFRepository<BlackWhiteList> _repository;
        public BlackWhiteListRepository(IEFRepository<BlackWhiteList> repository)
        {
            _repository = repository;
        }

        public async Task<bool> Add(BlackWhiteListModel model)
        {
            using (var context = DataContext.Instance())
            {
                context.BlackWhiteList.Add(ToEntity(model));
                return await context.SaveChangesAsync() > 0;
            }
        }

        public async Task<List<BlackWhiteListModel>> GetList()
        {
            using (var context = DataContext.Instance())
            {
                var list = await context.BlackWhiteList.Where(p => p.IsDeleted == false).ToListAsync();
                return list.Select(p => ToModel(p)).ToList();
            }
        }

        public Task<bool> DeleteById(List<int> ids)
        {
            using (var context = DataContext.Instance())
            {
                var result = _repository.Instance(context).ModifyBy(new BlackWhiteList { IsDeleted = true }, p => ids.Contains(p.Id), "IsDeleted") > 0;
                return Task.FromResult(result);
            }
        }

        public async Task<Page<BlackWhiteListModel>> GetPageAsync(BlackWhiteListQuery query)
        {
            using (var context = DataContext.Instance())
            {
                var result = new Page<BlackWhiteListModel>();
                var entities = await _repository.Instance(context).GetPageList(query.Page, query.PageSize, x => x
                .WhereIF(query.Id.HasValue, e => e.Id == query.Id)
                .WhereIF(!query.Ids.IsNullOrEmpty(), e => query.Ids.Contains(e.Id))
                .WhereIF(!query.RoutePathPattern.IsNullOrEmpty(), e => e.RoutePathPattern.Contains(query.RoutePathPattern))
                .WhereIF(query.Status!=null, e => e.Status==query.Status)
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
        }

       public  async Task<List<BlackWhiteListModel>> GetListByIds(List<int> ids)
        {
            using (var context = DataContext.Instance())
            {
                var list =await (from q in context.BlackWhiteList where ids.Contains(q.Id) select q).ToListAsync();
                return  list.Select(p => ToModel(p)).ToList();
            }
        }

        public async Task<bool> Modify(BlackWhiteListModel model)
        {
            using (var context = DataContext.Instance())
            {
                var entity = ToEntity(model);
                var result = _repository.Instance(context).ModifyBy(entity, p => p.Id == model.Id, "RoutePathPattern", "WhiteList", "BlackList", "Status", "Remark", "Updater", "UpdateDate") > 0;
                return await Task.FromResult(result);
            }
        }

        public async Task<bool> Disable(List<int> ids)
        {
            using (var context = DataContext.Instance())
            {
                var result = _repository.Instance(context).ModifyBy(new BlackWhiteList { Status = 0 }, p => ids.Contains(p.Id), "Status") > 0;
                return await Task.FromResult(result);
            }
        }

        public async Task<bool> Enable(List<int> ids)
        {
            using (var context = DataContext.Instance())
            {
                var result = _repository.Instance(context).ModifyBy(new BlackWhiteList { Status = 1 }, p => ids.Contains(p.Id), "Status") > 0;
                return await Task.FromResult(result);
            }
        }

        private BlackWhiteList ToEntity(BlackWhiteListModel model)
        {

            return model == null ? default : new BlackWhiteList
            {
                Id = model.Id,
                CreateDate = DateTime.Now,
                BlackList = model.BlackList,
                RoutePathPattern = model.RoutePathPattern,
                Status = model.Status,
                Remark = model.Remark,
                WhiteList = model.WhiteList,
                UpdateDate = DateTime.Now,
                Creater = IdentityContext.Get()?.UserId,
                Updater = IdentityContext.Get()?.UserId
            };
        }


        private BlackWhiteListModel ToModel(BlackWhiteList entity)
        {

            return entity == null ? default : new BlackWhiteListModel
            {
                Id = entity.Id,
                CreateDate = entity.CreateDate,
                BlackList = entity.BlackList,
                RoutePathPattern = entity.RoutePathPattern,
                Status = entity.Status,
                Remark = entity.Remark,
                WhiteList = entity.WhiteList,
                UpdateDate = entity.UpdateDate,
            };
        }
    }
}
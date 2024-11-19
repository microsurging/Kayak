using Kayak.Core.Common.Repsitories;
using Kayak.DataAccess.DeviceData.Entities;
using Kayak.DataAccess.DeviceData;
using Kayak.IModuleServices.DeviceManage.Models;
using Kayak.IModuleServices.DeviceManage.Queries;
using Surging.Core.CPlatform.Ioc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kayak.Core.Common.Extensions;
using Microsoft.EntityFrameworkCore;
using Kayak.Core.Common.Context;

namespace Kayak.Modules.DeviceManage.Repositories
{
    public class FunctionParameterRepository : BaseRepository, ISingleInstance
    {
        private readonly IEFRepository<FunctionParameter> _repository;
        public FunctionParameterRepository(IEFRepository<FunctionParameter> repository)
        {
            _repository = repository;
        }

        public async Task<bool> AddList(List<FunctionParameterModel> model)
        {
            using (var context = DataContext.Instance())
            {
                context.FunctionParameter.AddRange(model.Select(p => ToEntity(p)).ToArray());
                return await context.SaveChangesAsync() > 0;
            }
        }

        public async Task<List<FunctionParameterModel>> Get(FunctionParameterQuery query)
        {
            using (var context = DataContext.Instance())
            {
                var queryable = context.FunctionParameter.Where(p => p.FunctionCode == query.FunctionCode);
                if (!query.ProductCode.IsNullOrEmpty())
                {
                    queryable = queryable.Where(p => p.ProductCode == query.ProductCode);
                }
                if (!query.DeviceCode.IsNullOrEmpty())
                {
                    queryable = queryable.Where(p => p.DeviceCode == query.DeviceCode);
                }
                var entities = await queryable.ToListAsync();
                return entities.Select(p => ToModel(p)).ToList();
            }
        }

        public async Task<List<FunctionParameterModel>> GetByFunctionId(int functionId, string parameterType)
        {
            using (var context = DataContext.Instance())
            {
                var entities = await context.FunctionParameter.AsNoTracking().Where(p => p.FunctionId == functionId &&  p.ParameterType==parameterType).ToListAsync();
                return entities.Select(p => ToModel(p)).ToList();
            }
        }

        public Task<bool> Modify(FunctionParameterModel model)
        {
            using (var context = DataContext.Instance())
            {
                var entity = ToEntity(model);
                var result = _repository.Instance(context).ModifyBy(entity, p => p.Id == model.Id, "Code", "ProductCode", "Name", "DeviceCode", "FunctionCode", "Constraint", "DataTypeValue", "Updater", "UpdateDate") > 0;
                return Task.FromResult(result);
            }
        }

        public async Task<bool> DelBatch(List<FunctionParameterModel> list)
        {
            using (var context = DataContext.Instance())
            {
                var ids = list.Select(p => p.Id).ToList();
                var result = await _repository.Instance(context).Delete(p => ids.Contains(p.Id)) > 0;
                return result;
            }
        }

        public FunctionParameterModel ToModel(FunctionParameter entity)
        {
            return entity == null ? default : new FunctionParameterModel
            {
                Id = entity.Id,
                ProductCode = entity.ProductCode,
                DeviceCode = entity.DeviceCode,
                Code = entity.Code,
                Constraint = entity.Constraint,
                DataTypeValue = entity.DataTypeValue,
                FunctionCode = entity.FunctionCode,
                FunctionId = entity.FunctionId,
                Name = entity.Name,
                ParameterType = entity.ParameterType,
                CreateDate = entity.CreateDate,
                UpdateDate = entity.UpdateDate,
            };
        }

        public FunctionParameter ToEntity(FunctionParameterModel model)
        {
            return model == null ? default : new FunctionParameter
            {
                ProductCode = model.ProductCode,
                DeviceCode = model.DeviceCode,
                Code = model.Code,
                Constraint = model.Constraint,
                DataTypeValue = model.DataTypeValue,
                FunctionCode = model.FunctionCode,
                FunctionId = model.FunctionId,
                Name = model.Name,
                ParameterType = model.ParameterType,
                CreateDate = model.CreateDate,
                UpdateDate = model.UpdateDate, 
                Creater = IdentityContext.Get()?.UserId,
                Updater = IdentityContext.Get()?.UserId,
            };
        }
    }
}

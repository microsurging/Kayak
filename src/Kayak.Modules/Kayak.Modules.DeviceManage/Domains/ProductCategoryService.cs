using Kayak.Core.Common.Enums;
using Kayak.Core.Common.Extensions;
using Kayak.Core.Common.Repsitories.Implementation;
using Kayak.Core.Common.Response;
using Kayak.IModuleServices.DeviceManage;
using Kayak.IModuleServices.DeviceManage.Models;
using Kayak.IModuleServices.DeviceManage.Queries;
using Kayak.Modules.DeviceManage.Repositories;
using Surging.Core.CPlatform.Ioc;
using Surging.Core.ProxyGenerator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Kayak.Modules.DeviceManage.Domains
{
    public class ProductCategoryService : ProxyServiceBase, IProductCategoryService, ISingleInstance
    {
        private readonly ProductCategoryRepsitory _repository;
        public ProductCategoryService(ProductCategoryRepsitory repository)
        { 
            _repository = repository;
        }
        public async Task<ApiResult<bool>> Add(ProductCategoryModel model)
        {
            var result = await _repository.Add(model);
            return ApiResult<bool>.Succeed(result);
        }

        public async Task<ApiResult<bool>> AddChildren(ProductCategoryModel model)
        {
            var result = await _repository.AddChildren(model);
            return ApiResult<bool>.Succeed(result);
        }

        public async Task<ApiResult<bool>> DeleteById(List<int> ids)
        {
            var result = await _repository.DeleteById(ids);
            return ApiResult<bool>.Succeed(result);
        }

        public async Task<ApiResult<Page<ProductCategoryModel>>> GetPageAsync(ProductCategoryQuery query)
        {
            var result = await _repository.GetPageAsync(query);
            return ApiResult<Page<ProductCategoryModel>>.Succeed(result);
        }

        public async Task<ApiResult<ProductCategoryModel>> GetProductCategory(int id)
        {
            var result = await _repository.GetProductCategory(id);
            return ApiResult<ProductCategoryModel>.Succeed(result);
        }

        public async Task<ApiResult<List<ProductCategoryModel>>> GetLastChild()
        {
            var result = await _repository.GetLastChild();
            return ApiResult<List<ProductCategoryModel>>.Succeed(result);
        }

        public async Task<ApiResult<List<ProductCategoryModel>>> GetProductCategoryByCondition(ProductCategoryQuery query)
        {
            var result = await _repository.GetProductCategoryByCondition(query);
            return ApiResult<List<ProductCategoryModel>>.Succeed(result);
        }

        public async Task<ApiResult<List<ProductCategoryModel>>> GetProductCategoryByIds(List<int> ids)
        {
            var result = await _repository.GetProductCategoryByIds(ids);
            return ApiResult<List<ProductCategoryModel>>.Succeed(result);
        }

        public async Task<ApiResult<bool>> Modify(ProductCategoryModel model)
        {
            var result = await _repository.Modify(model);
            return ApiResult<bool>.Succeed(result);
        }

        public async Task<ApiResult<bool>> Validate(ProductCategoryModel model)
        {
            var message = "";
            if (!model.Code.IsNullOrEmpty())
            {
                var result = await _repository.ExistsModelByCode(model.CategoryId,model.Code);
                message = result ? "分类编码已存在" : message;
            }
            return message.IsNullOrEmpty() ? ApiResult<bool>.Succeed(true) : ApiResult<bool>.Failure(EnumReturnCode.CUSTOM_ERROR, message);
        }
    }
}

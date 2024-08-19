using Kayak.Core.Common.Enums;
using Kayak.Core.Common.Extensions;
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
    public class ProductService : ProxyServiceBase, IProductService, ISingleInstance
    {
        private readonly ProductRepository _repository;
        public ProductService(ProductRepository repository)
        {
            _repository = repository;
        }

        public async Task<ApiResult<bool>> Add(ProductModel model)
        { 
            var result = await _repository.Add(model);
            return ApiResult<bool>.Succeed(result); ;
        }

        public async Task<ApiResult<bool>> DeleteById(List<int> ids)
        {
            var result = await _repository.DeleteById(ids);
            return ApiResult<bool>.Succeed(result);
        }

        public async Task<ApiResult<Page<ProductModel>>> GetPageAsync(ProductQuery query)
        {
            var result = await _repository.GetPageAsync(query);
            return ApiResult<Page<ProductModel>>.Succeed(result);
        }

        public async Task<ApiResult<ProductModel>> GetProduct(int id)
        {
            var result = await _repository.GetProduct(id);
            return ApiResult<ProductModel>.Succeed(result);
        }

        public async Task<ApiResult<List<ProductModel>>> GetProductByCondition(ProductQuery query)
        {
            var result = await _repository.GetProductByCondition(query);
            return ApiResult<List<ProductModel>>.Succeed(result);
        }

        public async Task<ApiResult<List<ProductModel>>> GetProducts()
        {
            var result = await _repository.GetProducts();
            return ApiResult<List<ProductModel>>.Succeed(result);
        }

        public async Task<ApiResult<bool>> Modify(ProductModel model)
        {

            var result = await _repository.Modify(model);
            return ApiResult<bool>.Succeed(result);
        }

        public async Task<ApiResult<bool>> Open(List<int> ids)
        {
            var result = await _repository.Open(ids);
            return ApiResult<bool>.Succeed(result);
        }

        public async Task<ApiResult<bool>> Stop(List<int> ids)
        {
            var result = await _repository.Stop(ids);
            return ApiResult<bool>.Succeed(result);
        }

        public async Task<ApiResult<bool>> Validate(ProductModel model)
        {
            var message = "";
            if (!model.ProductCode.IsNullOrEmpty())
            {
                var result = await _repository.ExistsModelByCode(model.ProductCode);
                message = result ? "产品编码已存在" : message;
            }

            return message.IsNullOrEmpty() ? ApiResult<bool>.Succeed(true) : ApiResult<bool>.Failure(EnumReturnCode.CUSTOM_ERROR, message);
        }
    }
}

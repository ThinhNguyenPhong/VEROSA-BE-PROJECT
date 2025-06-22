using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.Extensions.Logging;
using VEROSA.Common.Exceptions;
using VEROSA.Common.Models.Pages;
using VEROSA.Common.Models.Request;
using VEROSA.Common.Models.Response;
using VEROSA.DataAccessLayer.Bases.UnitOfWork;

namespace VEROSA.BussinessLogicLayer.Services.Product
{
    public class ProductService : IProductService
    {
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;
        private readonly ILogger<ProductService> _logger;

        public ProductService(IUnitOfWork uow, IMapper mapper, ILogger<ProductService> logger)
        {
            _uow = uow;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<IEnumerable<ProductResponse>> GetAllAsync()
        {
            try
            {
                var list = await _uow.Products.GetAllAsync();
                return _mapper.Map<IEnumerable<ProductResponse>>(list);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to get products");
                throw new AppException(ErrorCode.INTERNAL_SERVER_ERROR);
            }
        }

        public async Task<ProductResponse?> GetByIdAsync(Guid id)
        {
            try
            {
                var e = await _uow.Products.GetByIdAsync(id);
                if (e == null)
                    return null;
                return _mapper.Map<ProductResponse>(e);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Failed to get product {id}");
                throw new AppException(ErrorCode.INTERNAL_SERVER_ERROR);
            }
        }

        public async Task<ProductResponse> CreateAsync(ProductRequest req)
        {
            try
            {
                var e = _mapper.Map<DataAccessLayer.Entities.Product>(req);
                e.Id = Guid.NewGuid();
                e.CreatedAt = DateTime.UtcNow;
                await _uow.Products.AddAsync(e);
                await _uow.CommitAsync();
                return _mapper.Map<ProductResponse>(e);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to create product");
                throw new AppException(ErrorCode.INTERNAL_SERVER_ERROR);
            }
        }

        public async Task<bool> UpdateAsync(Guid id, ProductRequest req)
        {
            try
            {
                var e = await _uow.Products.GetByIdAsync(id);
                if (e == null)
                    return false;
                _mapper.Map(req, e);
                _uow.Products.Update(e);
                await _uow.CommitAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Failed to update product {id}");
                throw new AppException(ErrorCode.INTERNAL_SERVER_ERROR);
            }
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            try
            {
                var e = await _uow.Products.GetByIdAsync(id);
                if (e == null)
                    return false;
                _uow.Products.Delete(e);
                await _uow.CommitAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Failed to delete product {id}");
                throw new AppException(ErrorCode.INTERNAL_SERVER_ERROR);
            }
        }

        public async Task<PageResult<ProductResponse>> GetWithParamsAsync(
            Guid? categoryId,
            string? name,
            string? sortBy,
            bool sortDesc,
            int pageNumber,
            int pageSize
        )
        {
            try
            {
                var all = (await _uow.Products.FindProductsAsync(categoryId, name)).ToList();
                if (!all.Any())
                    throw new AppException(ErrorCode.LIST_EMPTY);

                IOrderedEnumerable<DataAccessLayer.Entities.Product> sorted;
                if (!string.IsNullOrWhiteSpace(sortBy))
                {
                    var prop = typeof(DataAccessLayer.Entities.Product).GetProperty(sortBy);
                    Func<DataAccessLayer.Entities.Product, object?> key = p =>
                        prop != null ? prop.GetValue(p) : null;
                    sorted = sortDesc ? all.OrderByDescending(key) : all.OrderBy(key);
                }
                else
                    sorted = all.OrderByDescending(p => p.CreatedAt);

                var paged = sorted.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();
                var dtos = _mapper.Map<List<ProductResponse>>(paged);
                return new PageResult<ProductResponse>(dtos, pageSize, pageNumber, all.Count);
            }
            catch (AppException)
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to get products with params");
                throw new AppException(ErrorCode.INTERNAL_SERVER_ERROR);
            }
        }
    }
}

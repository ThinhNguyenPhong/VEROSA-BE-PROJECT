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
    public class ProductCategoryService : IProductCategoryService
    {
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;
        private readonly ILogger<ProductCategoryService> _logger;

        public ProductCategoryService(
            IUnitOfWork uow,
            IMapper mapper,
            ILogger<ProductCategoryService> logger
        )
        {
            _uow = uow;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<IEnumerable<ProductCategoryResponse>> GetAllAsync()
        {
            try
            {
                var list = await _uow.ProductCategories.GetAllAsync();
                return _mapper.Map<IEnumerable<ProductCategoryResponse>>(list);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to get categories");
                throw new AppException(ErrorCode.INTERNAL_SERVER_ERROR);
            }
        }

        public async Task<ProductCategoryResponse?> GetByIdAsync(Guid id)
        {
            try
            {
                var e = await _uow.ProductCategories.GetByIdAsync(id);
                if (e == null)
                    return null;
                return _mapper.Map<ProductCategoryResponse>(e);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Failed to get category by id {id}");
                throw new AppException(ErrorCode.INTERNAL_SERVER_ERROR);
            }
        }

        public async Task<ProductCategoryResponse> CreateAsync(ProductCategoryRequest req)
        {
            try
            {
                var entity = _mapper.Map<DataAccessLayer.Entities.ProductCategory>(req);
                entity.Id = Guid.NewGuid();
                await _uow.ProductCategories.AddAsync(entity);
                await _uow.CommitAsync();
                return _mapper.Map<ProductCategoryResponse>(entity);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to create category");
                throw new AppException(ErrorCode.INTERNAL_SERVER_ERROR);
            }
        }

        public async Task<bool> UpdateAsync(Guid id, ProductCategoryRequest req)
        {
            try
            {
                var e = await _uow.ProductCategories.GetByIdAsync(id);
                if (e == null)
                    return false;
                _mapper.Map(req, e);
                _uow.ProductCategories.Update(e);
                await _uow.CommitAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Failed to update category {id}");
                throw new AppException(ErrorCode.INTERNAL_SERVER_ERROR);
            }
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            try
            {
                var e = await _uow.ProductCategories.GetByIdAsync(id);
                if (e == null)
                    return false;
                _uow.ProductCategories.Delete(e);
                await _uow.CommitAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Failed to delete category {id}");
                throw new AppException(ErrorCode.INTERNAL_SERVER_ERROR);
            }
        }

        public async Task<PageResult<ProductCategoryResponse>> GetWithParamsAsync(
            string? name,
            string? sortBy,
            bool sortDesc,
            int pageNumber,
            int pageSize
        )
        {
            try
            {
                var all = (await _uow.ProductCategories.FindCategoriesAsync(name)).ToList();
                if (!all.Any())
                    throw new AppException(ErrorCode.LIST_EMPTY);

                IOrderedEnumerable<DataAccessLayer.Entities.ProductCategory> sorted;
                if (!string.IsNullOrWhiteSpace(sortBy))
                {
                    var prop = typeof(DataAccessLayer.Entities.ProductCategory).GetProperty(sortBy);
                    Func<DataAccessLayer.Entities.ProductCategory, object?> key = c =>
                        prop != null ? prop.GetValue(c) : null;
                    sorted = sortDesc ? all.OrderByDescending(key) : all.OrderBy(key);
                }
                else
                    sorted = all.OrderBy(c => c.Name);

                var paged = sorted.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();
                var dtos = _mapper.Map<List<ProductCategoryResponse>>(paged);
                return new PageResult<ProductCategoryResponse>(
                    dtos,
                    pageSize,
                    pageNumber,
                    all.Count
                );
            }
            catch (AppException)
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to get categories with params");
                throw new AppException(ErrorCode.INTERNAL_SERVER_ERROR);
            }
        }
    }
}

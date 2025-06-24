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

namespace VEROSA.BussinessLogicLayer.Services.Review
{
    public class ReviewService : IReviewService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<ReviewService> _logger;

        public ReviewService(IUnitOfWork unitOfWork, IMapper mapper, ILogger<ReviewService> logger)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<IEnumerable<ReviewResponse>> GetAllAsync()
        {
            try
            {
                var list = await _unitOfWork.Reviews.GetAllAsync();
                return _mapper.Map<IEnumerable<ReviewResponse>>(list);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to get all reviews");
                throw new AppException(ErrorCode.INTERNAL_SERVER_ERROR);
            }
        }

        public async Task<ReviewResponse?> GetByIdAsync(Guid id)
        {
            try
            {
                var e = await _unitOfWork.Reviews.GetByIdAsync(id);
                if (e == null)
                    return null;
                return _mapper.Map<ReviewResponse>(e);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Failed to get review {id}");
                throw new AppException(ErrorCode.INTERNAL_SERVER_ERROR);
            }
        }

        public async Task<ReviewResponse> CreateAsync(ReviewRequest req)
        {
            try
            {
                var e = _mapper.Map<DataAccessLayer.Entities.Review>(req);
                e.Id = Guid.NewGuid();
                e.CreatedAt = DateTime.UtcNow;
                await _unitOfWork.Reviews.AddAsync(e);
                await _unitOfWork.CommitAsync();
                return _mapper.Map<ReviewResponse>(e);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to create review");
                throw new AppException(ErrorCode.INTERNAL_SERVER_ERROR);
            }
        }

        public async Task<bool> UpdateAsync(Guid id, ReviewRequest req)
        {
            try
            {
                var e = await _unitOfWork.Reviews.GetByIdAsync(id);
                if (e == null)
                    return false;
                _mapper.Map(req, e);
                _unitOfWork.Reviews.Update(e);
                await _unitOfWork.CommitAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Failed to update review {id}");
                throw new AppException(ErrorCode.INTERNAL_SERVER_ERROR);
            }
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            try
            {
                var e = await _unitOfWork.Reviews.GetByIdAsync(id);
                if (e == null)
                    return false;
                _unitOfWork.Reviews.Delete(e);
                await _unitOfWork.CommitAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Failed to delete review {id}");
                throw new AppException(ErrorCode.INTERNAL_SERVER_ERROR);
            }
        }

        public async Task<PageResult<ReviewResponse>> GetWithParamsAsync(
            Guid? productId,
            Guid? customerId,
            int? minRating,
            int? maxRating,
            string? sortBy,
            bool sortDesc,
            int pageNumber,
            int pageSize
        )
        {
            try
            {
                var all = (
                    await _unitOfWork.Reviews.FindReviewsAsync(
                        productId,
                        customerId,
                        minRating,
                        maxRating
                    )
                ).ToList();
                if (!all.Any())
                    throw new AppException(ErrorCode.LIST_EMPTY);

                IOrderedEnumerable<DataAccessLayer.Entities.Review> sorted;
                if (!string.IsNullOrWhiteSpace(sortBy))
                {
                    var prop = typeof(DataAccessLayer.Entities.Review).GetProperty(sortBy);
                    Func<DataAccessLayer.Entities.Review, object?> key = r =>
                        prop != null ? prop.GetValue(r) : null;
                    sorted = sortDesc ? all.OrderByDescending(key) : all.OrderBy(key);
                }
                else
                    sorted = all.OrderByDescending(r => r.CreatedAt);

                var paged = sorted.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();

                var dtos = _mapper.Map<List<ReviewResponse>>(paged);
                return new PageResult<ReviewResponse>(dtos, pageSize, pageNumber, all.Count);
            }
            catch (AppException)
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to get reviews with params");
                throw new AppException(ErrorCode.INTERNAL_SERVER_ERROR);
            }
        }
    }
}

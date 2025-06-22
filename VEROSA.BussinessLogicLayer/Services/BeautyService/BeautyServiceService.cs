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

namespace VEROSA.BussinessLogicLayer.Services.BeautyService
{
    public class BeautyServiceService : IBeautyServiceService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<BeautyServiceService> _logger;

        public BeautyServiceService(
            IUnitOfWork unitOfWork,
            IMapper mapper,
            ILogger<BeautyServiceService> logger
        )
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<IEnumerable<BeautyServiceResponse>> GetAllAsync()
        {
            try
            {
                var list = await _unitOfWork.BeautyServices.GetAllAsync();
                return _mapper.Map<IEnumerable<BeautyServiceResponse>>(list);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to get all beauty services");
                throw new AppException(ErrorCode.INTERNAL_SERVER_ERROR);
            }
        }

        public async Task<BeautyServiceResponse?> GetByIdAsync(Guid id)
        {
            try
            {
                var entity = await _unitOfWork.BeautyServices.GetByIdAsync(id);
                if (entity == null)
                    return null;
                return _mapper.Map<BeautyServiceResponse>(entity);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Failed to get beauty service by id {id}");
                throw new AppException(ErrorCode.INTERNAL_SERVER_ERROR);
            }
        }

        public async Task<BeautyServiceResponse> CreateAsync(BeautyServiceRequest request)
        {
            try
            {
                var entity = _mapper.Map<DataAccessLayer.Entities.BeautyService>(request);
                entity.Id = Guid.NewGuid();
                entity.CreatedAt = DateTime.UtcNow;

                await _unitOfWork.BeautyServices.AddAsync(entity);
                await _unitOfWork.CommitAsync();

                return _mapper.Map<BeautyServiceResponse>(entity);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to create beauty service");
                throw new AppException(ErrorCode.INTERNAL_SERVER_ERROR);
            }
        }

        public async Task<bool> UpdateAsync(Guid id, BeautyServiceRequest request)
        {
            try
            {
                var entity = await _unitOfWork.BeautyServices.GetByIdAsync(id);
                if (entity == null)
                    return false;

                _mapper.Map(request, entity);
                _unitOfWork.BeautyServices.Update(entity);
                await _unitOfWork.CommitAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Failed to update beauty service id {id}");
                throw new AppException(ErrorCode.INTERNAL_SERVER_ERROR);
            }
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            try
            {
                var entity = await _unitOfWork.BeautyServices.GetByIdAsync(id);
                if (entity == null)
                    return false;

                _unitOfWork.BeautyServices.Delete(entity);
                await _unitOfWork.CommitAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Failed to delete beauty service id {id}");
                throw new AppException(ErrorCode.INTERNAL_SERVER_ERROR);
            }
        }

        public async Task<PageResult<BeautyServiceResponse>> GetWithParamsAsync(
            string? name,
            decimal? minPrice,
            decimal? maxPrice,
            string? sortBy,
            bool sortDescending,
            int pageNumber,
            int pageSize
        )
        {
            try
            {
                var all = await _unitOfWork.BeautyServices.FindBeautyServicesAsync(
                    name,
                    minPrice,
                    maxPrice
                );
                if (!all.Any())
                    throw new AppException(ErrorCode.LIST_EMPTY);

                IOrderedEnumerable<DataAccessLayer.Entities.BeautyService> sorted;
                if (!string.IsNullOrWhiteSpace(sortBy))
                {
                    var prop = typeof(DataAccessLayer.Entities.BeautyService).GetProperty(sortBy);
                    Func<DataAccessLayer.Entities.BeautyService, object?> keySel = s =>
                        prop != null ? prop.GetValue(s) : null;
                    sorted = sortDescending ? all.OrderByDescending(keySel) : all.OrderBy(keySel);
                }
                else
                {
                    sorted = all.OrderByDescending(s => s.CreatedAt);
                }

                var paged = sorted.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();

                var dtos = _mapper.Map<List<BeautyServiceResponse>>(paged);
                return new PageResult<BeautyServiceResponse>(
                    dtos,
                    pageSize,
                    pageNumber,
                    all.Count()
                );
            }
            catch (AppException)
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to get beauty services with params");
                throw new AppException(ErrorCode.INTERNAL_SERVER_ERROR);
            }
        }
    }
}

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

namespace VEROSA.BussinessLogicLayer.Services.Address
{
    public class AddressService : IAddressService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<AddressService> _logger;

        public AddressService(
            IUnitOfWork unitOfWork,
            IMapper mapper,
            ILogger<AddressService> logger
        )
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<IEnumerable<AddressResponse>> GetAllAsync()
        {
            try
            {
                var list = await _unitOfWork.Addresses.GetAllAsync();
                return _mapper.Map<IEnumerable<AddressResponse>>(list);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to get all addresses");
                throw new AppException(ErrorCode.INTERNAL_SERVER_ERROR);
            }
        }

        public async Task<AddressResponse?> GetByIdAsync(Guid id)
        {
            try
            {
                var entity = await _unitOfWork.Addresses.GetByIdAsync(id);
                if (entity == null)
                    return null;
                return _mapper.Map<AddressResponse>(entity);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Failed to get address by id {id}");
                throw new AppException(ErrorCode.INTERNAL_SERVER_ERROR);
            }
        }

        public async Task<AddressResponse> CreateAsync(AddressRequest request)
        {
            try
            {
                var entity = _mapper.Map<DataAccessLayer.Entities.Address>(request);
                entity.Id = Guid.NewGuid();
                entity.CreatedAt = DateTime.UtcNow;

                await _unitOfWork.Addresses.AddAsync(entity);
                await _unitOfWork.CommitAsync();

                return _mapper.Map<AddressResponse>(entity);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to create address");
                throw new AppException(ErrorCode.INTERNAL_SERVER_ERROR);
            }
        }

        public async Task<bool> UpdateAsync(Guid id, AddressRequest request)
        {
            try
            {
                var entity = await _unitOfWork.Addresses.GetByIdAsync(id);
                if (entity == null)
                    return false;

                _mapper.Map(request, entity);
                _unitOfWork.Addresses.Update(entity);
                await _unitOfWork.CommitAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Failed to update address id {id}");
                throw new AppException(ErrorCode.INTERNAL_SERVER_ERROR);
            }
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            try
            {
                var entity = await _unitOfWork.Addresses.GetByIdAsync(id);
                if (entity == null)
                    return false;

                _unitOfWork.Addresses.Delete(entity);
                await _unitOfWork.CommitAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Failed to delete address id {id}");
                throw new AppException(ErrorCode.INTERNAL_SERVER_ERROR);
            }
        }

        public async Task<PageResult<AddressResponse>> GetWithParamsAsync(
            Guid? accountId,
            string? street,
            string? city,
            string? district,
            string? country,
            string? sortBy,
            bool sortDescending,
            int pageNumber,
            int pageSize
        )
        {
            try
            {
                var all = await _unitOfWork.Addresses.FindAddressesAsync(
                    accountId,
                    street,
                    city,
                    district,
                    country
                );
                if (!all.Any())
                    throw new AppException(ErrorCode.LIST_EMPTY);

                // Dynamic sort
                IOrderedEnumerable<DataAccessLayer.Entities.Address> sorted;
                if (!string.IsNullOrWhiteSpace(sortBy))
                {
                    var prop = typeof(DataAccessLayer.Entities.Address).GetProperty(sortBy);
                    Func<DataAccessLayer.Entities.Address, object?> keySel = a =>
                        prop != null ? prop.GetValue(a) : null;

                    sorted = sortDescending ? all.OrderByDescending(keySel) : all.OrderBy(keySel);
                }
                else
                {
                    sorted = all.OrderByDescending(a => a.CreatedAt);
                }

                // Paging
                var paged = sorted.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();

                var dtos = _mapper.Map<List<AddressResponse>>(paged);
                return new PageResult<AddressResponse>(dtos, pageSize, pageNumber, all.Count());
            }
            catch (AppException)
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to get addresses with params");
                throw new AppException(ErrorCode.INTERNAL_SERVER_ERROR);
            }
        }
    }
}

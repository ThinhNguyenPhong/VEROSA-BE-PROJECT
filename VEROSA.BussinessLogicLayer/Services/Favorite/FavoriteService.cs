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

namespace VEROSA.BussinessLogicLayer.Services.Favorite
{
    public class FavoriteService : IFavoriteService
    {
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;
        private readonly ILogger<FavoriteService> _logger;

        public FavoriteService(IUnitOfWork uow, IMapper mapper, ILogger<FavoriteService> logger)
        {
            _uow = uow;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<IEnumerable<FavoriteResponse>> GetAllAsync()
        {
            try
            {
                var list = await _uow.Favorites.GetAllAsync();
                return _mapper.Map<IEnumerable<FavoriteResponse>>(list);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to get favorites");
                throw new AppException(ErrorCode.INTERNAL_SERVER_ERROR);
            }
        }

        public async Task<FavoriteResponse?> GetByIdAsync(Guid id)
        {
            try
            {
                var e = await _uow.Favorites.GetByIdAsync(id);
                if (e == null)
                    return null;
                return _mapper.Map<FavoriteResponse>(e);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Failed to get favorite {id}");
                throw new AppException(ErrorCode.INTERNAL_SERVER_ERROR);
            }
        }

        public async Task<FavoriteResponse> CreateAsync(FavoriteRequest req)
        {
            try
            {
                var e = _mapper.Map<DataAccessLayer.Entities.Favorite>(req);
                e.Id = Guid.NewGuid();
                e.CreatedAt = DateTime.UtcNow;
                await _uow.Favorites.AddAsync(e);
                await _uow.CommitAsync();
                return _mapper.Map<FavoriteResponse>(e);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to create favorite");
                throw new AppException(ErrorCode.INTERNAL_SERVER_ERROR);
            }
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            try
            {
                var e = await _uow.Favorites.GetByIdAsync(id);
                if (e == null)
                    return false;
                _uow.Favorites.Delete(e);
                await _uow.CommitAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Failed to delete favorite {id}");
                throw new AppException(ErrorCode.INTERNAL_SERVER_ERROR);
            }
        }

        public async Task<PageResult<FavoriteResponse>> GetWithParamsAsync(
            Guid? customerId,
            Guid? productId,
            string? sortBy,
            bool sortDesc,
            int pageNumber,
            int pageSize
        )
        {
            try
            {
                var all = (await _uow.Favorites.FindFavoritesAsync(customerId, productId)).ToList();
                if (!all.Any())
                    throw new AppException(ErrorCode.LIST_EMPTY);

                IOrderedEnumerable<DataAccessLayer.Entities.Favorite> sorted;
                if (!string.IsNullOrWhiteSpace(sortBy))
                {
                    var prop = typeof(DataAccessLayer.Entities.Favorite).GetProperty(sortBy);
                    Func<DataAccessLayer.Entities.Favorite, object?> key = f =>
                        prop != null ? prop.GetValue(f) : null;
                    sorted = sortDesc ? all.OrderByDescending(key) : all.OrderBy(key);
                }
                else
                    sorted = all.OrderByDescending(f => f.CreatedAt);

                var page = sorted.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();

                var dtos = _mapper.Map<List<FavoriteResponse>>(page);
                return new PageResult<FavoriteResponse>(dtos, pageSize, pageNumber, all.Count);
            }
            catch (AppException)
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to get favorites with params");
                throw new AppException(ErrorCode.INTERNAL_SERVER_ERROR);
            }
        }
    }
}

using AutoMapper;
using Microsoft.Extensions.Logging;
using VEROSA.Common.Enums;
using VEROSA.Common.Exceptions;
using VEROSA.Common.Models.Pages;
using VEROSA.Common.Models.Request;
using VEROSA.Common.Models.Response;
using VEROSA.DataAccessLayer.Bases.UnitOfWork;

namespace VEROSA.BussinessLogicLayer.Services.SupportTicket
{
    public class SupportTicketService : ISupportTicketService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<SupportTicketService> _logger;

        public SupportTicketService(
            IUnitOfWork unitOfWork,
            IMapper mapper,
            ILogger<SupportTicketService> logger
        )
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<IEnumerable<SupportTicketResponse>> GetAllAsync()
        {
            try
            {
                var list = await _unitOfWork.SupportTickets.GetAllAsync();
                return _mapper.Map<IEnumerable<SupportTicketResponse>>(list);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to get tickets");
                throw new AppException(ErrorCode.INTERNAL_SERVER_ERROR);
            }
        }

        public async Task<SupportTicketResponse?> GetByIdAsync(Guid id)
        {
            try
            {
                var e = await _unitOfWork.SupportTickets.GetByIdAsync(id);
                if (e == null)
                    return null;
                return _mapper.Map<SupportTicketResponse>(e);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Failed to get ticket {id}");
                throw new AppException(ErrorCode.INTERNAL_SERVER_ERROR);
            }
        }

        public async Task<SupportTicketResponse> CreateAsync(SupportTicketRequest req)
        {
            try
            {
                var e = _mapper.Map<DataAccessLayer.Entities.SupportTicket>(req);
                e.Id = Guid.NewGuid();
                e.CreatedAt = DateTime.UtcNow;
                e.UpdatedAt = DateTime.UtcNow;
                await _unitOfWork.SupportTickets.AddAsync(e);
                await _unitOfWork.CommitAsync();
                return _mapper.Map<SupportTicketResponse>(e);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to create ticket");
                throw new AppException(ErrorCode.INTERNAL_SERVER_ERROR);
            }
        }

        public async Task<bool> UpdateAsync(Guid id, SupportTicketRequest req)
        {
            try
            {
                var e = await _unitOfWork.SupportTickets.GetByIdAsync(id);
                if (e == null)
                    return false;
                _mapper.Map(req, e);
                e.UpdatedAt = DateTime.UtcNow;
                _unitOfWork.SupportTickets.Update(e);
                await _unitOfWork.CommitAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Failed to update ticket {id}");
                throw new AppException(ErrorCode.INTERNAL_SERVER_ERROR);
            }
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            try
            {
                var e = await _unitOfWork.SupportTickets.GetByIdAsync(id);
                if (e == null)
                    return false;
                _unitOfWork.SupportTickets.Delete(e);
                await _unitOfWork.CommitAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Failed to delete ticket {id}");
                throw new AppException(ErrorCode.INTERNAL_SERVER_ERROR);
            }
        }

        public async Task<PageResult<SupportTicketResponse>> GetWithParamsAsync(
            Guid? customerId,
            SupportStatus? status,
            string? sortBy,
            bool sortDesc,
            int pageNumber,
            int pageSize
        )
        {
            try
            {
                var all = (
                    await _unitOfWork.SupportTickets.FindTicketsAsync(customerId, status)
                ).ToList();
                if (!all.Any())
                    throw new AppException(ErrorCode.LIST_EMPTY);

                IOrderedEnumerable<DataAccessLayer.Entities.SupportTicket> sorted;
                if (!string.IsNullOrWhiteSpace(sortBy))
                {
                    var prop = typeof(DataAccessLayer.Entities.SupportTicket).GetProperty(sortBy);
                    Func<DataAccessLayer.Entities.SupportTicket, object?> key = t =>
                        prop != null ? prop.GetValue(t) : null;
                    sorted = sortDesc ? all.OrderByDescending(key) : all.OrderBy(key);
                }
                else
                    sorted = all.OrderByDescending(t => t.UpdatedAt);

                var paged = sorted.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();

                var dtos = _mapper.Map<List<SupportTicketResponse>>(paged);
                return new PageResult<SupportTicketResponse>(dtos, pageSize, pageNumber, all.Count);
            }
            catch (AppException)
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to get tickets with params");
                throw new AppException(ErrorCode.INTERNAL_SERVER_ERROR);
            }
        }
    }
}

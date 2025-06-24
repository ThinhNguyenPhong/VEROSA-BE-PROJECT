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

namespace VEROSA.BussinessLogicLayer.Services.Contact
{
    public class ContactService : IContactService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<ContactService> _logger;

        public ContactService(
            IUnitOfWork unitOfWork,
            IMapper mapper,
            ILogger<ContactService> logger
        )
        {
            _unitOfWork = _unitOfWork;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<IEnumerable<ContactResponse>> GetAllAsync()
        {
            try
            {
                var list = await _unitOfWork.Contacts.GetAllAsync();
                return _mapper.Map<IEnumerable<ContactResponse>>(list);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to get all contacts");
                throw new AppException(ErrorCode.INTERNAL_SERVER_ERROR);
            }
        }

        public async Task<ContactResponse?> GetByIdAsync(Guid id)
        {
            try
            {
                var e = await _unitOfWork.Contacts.GetByIdAsync(id);
                if (e == null)
                    return null;
                return _mapper.Map<ContactResponse>(e);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Failed to get contact {id}");
                throw new AppException(ErrorCode.INTERNAL_SERVER_ERROR);
            }
        }

        public async Task<ContactResponse> CreateAsync(ContactRequest req)
        {
            try
            {
                var e = _mapper.Map<DataAccessLayer.Entities.Contact>(req);
                e.Id = Guid.NewGuid();
                e.CreatedAt = DateTime.UtcNow;
                await _unitOfWork.Contacts.AddAsync(e);
                await _unitOfWork.CommitAsync();
                return _mapper.Map<ContactResponse>(e);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to create contact");
                throw new AppException(ErrorCode.INTERNAL_SERVER_ERROR);
            }
        }

        public async Task<bool> UpdateAsync(Guid id, ContactRequest req)
        {
            try
            {
                var e = await _unitOfWork.Contacts.GetByIdAsync(id);
                if (e == null)
                    return false;
                _mapper.Map(req, e);
                _unitOfWork.Contacts.Update(e);
                await _unitOfWork.CommitAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Failed to update contact {id}");
                throw new AppException(ErrorCode.INTERNAL_SERVER_ERROR);
            }
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            try
            {
                var e = await _unitOfWork.Contacts.GetByIdAsync(id);
                if (e == null)
                    return false;
                _unitOfWork.Contacts.Delete(e);
                await _unitOfWork.CommitAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Failed to delete contact {id}");
                throw new AppException(ErrorCode.INTERNAL_SERVER_ERROR);
            }
        }

        public async Task<PageResult<ContactResponse>> GetWithParamsAsync(
            string? name,
            string? email,
            bool? isResolved,
            string? sortBy,
            bool sortDesc,
            int pageNumber,
            int pageSize
        )
        {
            try
            {
                var all = (
                    await _unitOfWork.Contacts.FindContactsAsync(name, email, isResolved)
                ).ToList();
                if (!all.Any())
                    throw new AppException(ErrorCode.LIST_EMPTY);

                IOrderedEnumerable<DataAccessLayer.Entities.Contact> sorted;
                if (!string.IsNullOrWhiteSpace(sortBy))
                {
                    var prop = typeof(DataAccessLayer.Entities.Contact).GetProperty(sortBy);
                    Func<DataAccessLayer.Entities.Contact, object?> key = c =>
                        prop != null ? prop.GetValue(c) : null;
                    sorted = sortDesc ? all.OrderByDescending(key) : all.OrderBy(key);
                }
                else
                    sorted = all.OrderByDescending(c => c.CreatedAt);

                var paged = sorted.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();

                var dtos = _mapper.Map<List<ContactResponse>>(paged);
                return new PageResult<ContactResponse>(dtos, pageSize, pageNumber, all.Count);
            }
            catch (AppException)
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to get contacts with params");
                throw new AppException(ErrorCode.INTERNAL_SERVER_ERROR);
            }
        }
    }
}

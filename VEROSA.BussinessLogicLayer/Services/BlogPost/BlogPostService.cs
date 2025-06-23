using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.Extensions.Logging;
using VEROSA.Common.Enums;
using VEROSA.Common.Exceptions;
using VEROSA.Common.Models.Pages;
using VEROSA.Common.Models.Request;
using VEROSA.Common.Models.Response;
using VEROSA.DataAccessLayer.Bases.UnitOfWork;

namespace VEROSA.BussinessLogicLayer.Services.BlogPost
{
    public class BlogPostService : IBlogPostService
    {
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;
        private readonly ILogger<BlogPostService> _logger;

        public BlogPostService(IUnitOfWork uow, IMapper mapper, ILogger<BlogPostService> logger)
        {
            _uow = uow;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<IEnumerable<BlogPostResponse>> GetAllAsync()
        {
            try
            {
                var list = await _uow.BlogPosts.GetAllAsync();
                return _mapper.Map<IEnumerable<BlogPostResponse>>(list);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to get all blog posts");
                throw new AppException(ErrorCode.INTERNAL_SERVER_ERROR);
            }
        }

        public async Task<BlogPostResponse?> GetByIdAsync(Guid id)
        {
            try
            {
                var e = await _uow.BlogPosts.GetByIdAsync(id);
                if (e == null)
                    return null;
                return _mapper.Map<BlogPostResponse>(e);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Failed to get blog post {id}");
                throw new AppException(ErrorCode.INTERNAL_SERVER_ERROR);
            }
        }

        public async Task<BlogPostResponse> CreateAsync(BlogPostRequest req)
        {
            try
            {
                var e = _mapper.Map<DataAccessLayer.Entities.BlogPost>(req);
                e.Id = Guid.NewGuid();
                e.PublishedAt = DateTime.UtcNow;
                await _uow.BlogPosts.AddAsync(e);
                await _uow.CommitAsync();
                return _mapper.Map<BlogPostResponse>(e);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to create blog post");
                throw new AppException(ErrorCode.INTERNAL_SERVER_ERROR);
            }
        }

        public async Task<bool> UpdateAsync(Guid id, BlogPostRequest req)
        {
            try
            {
                var e = await _uow.BlogPosts.GetByIdAsync(id);
                if (e == null)
                    return false;
                _mapper.Map(req, e);
                _uow.BlogPosts.Update(e);
                await _uow.CommitAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Failed to update blog post {id}");
                throw new AppException(ErrorCode.INTERNAL_SERVER_ERROR);
            }
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            try
            {
                var e = await _uow.BlogPosts.GetByIdAsync(id);
                if (e == null)
                    return false;
                _uow.BlogPosts.Delete(e);
                await _uow.CommitAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Failed to delete blog post {id}");
                throw new AppException(ErrorCode.INTERNAL_SERVER_ERROR);
            }
        }

        public async Task<PageResult<BlogPostResponse>> GetWithParamsAsync(
            string? title,
            PostType? type,
            Guid? authorId,
            string? sortBy,
            bool sortDesc,
            int pageNumber,
            int pageSize
        )
        {
            try
            {
                var all = (await _uow.BlogPosts.FindBlogPostsAsync(title, type, authorId)).ToList();
                if (!all.Any())
                    throw new AppException(ErrorCode.LIST_EMPTY);

                IOrderedEnumerable<DataAccessLayer.Entities.BlogPost> sorted;
                if (!string.IsNullOrWhiteSpace(sortBy))
                {
                    var prop = typeof(DataAccessLayer.Entities.BlogPost).GetProperty(sortBy);
                    Func<DataAccessLayer.Entities.BlogPost, object?> key = b =>
                        prop != null ? prop.GetValue(b) : null;
                    sorted = sortDesc ? all.OrderByDescending(key) : all.OrderBy(key);
                }
                else
                    sorted = all.OrderByDescending(b => b.PublishedAt);

                var paged = sorted.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();
                var dtos = _mapper.Map<List<BlogPostResponse>>(paged);
                return new PageResult<BlogPostResponse>(dtos, pageSize, pageNumber, all.Count);
            }
            catch (AppException)
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to get blog posts with params");
                throw new AppException(ErrorCode.INTERNAL_SERVER_ERROR);
            }
        }
    }
}

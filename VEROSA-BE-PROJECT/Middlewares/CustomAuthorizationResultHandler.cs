using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Policy;
using Microsoft.AspNetCore.Http;
using VEROSA.Common.Models.ApiResponse;
using VEROSA.Common.Models.Pages;
using VEROSA.Common.Models.Response;

namespace VEROSA_BE_PROJECT.Middlewares
{
    public class CustomAuthorizationResultHandler : IAuthorizationMiddlewareResultHandler
    {
        private readonly AuthorizationMiddlewareResultHandler _defaultHandler = new();

        public async Task HandleAsync(
            RequestDelegate next,
            HttpContext context,
            AuthorizationPolicy policy,
            PolicyAuthorizationResult authorizeResult
        )
        {
            if (authorizeResult.Succeeded)
            {
                // authorize OK → tiếp pipeline bình thường
                await _defaultHandler.HandleAsync(next, context, policy, authorizeResult);
                return;
            }

            if (authorizeResult.Forbidden)
            {
                // authenticated nhưng không đủ role → 403 + JSON ApiResponse
                context.Response.StatusCode = StatusCodes.Status403Forbidden;
                context.Response.ContentType = "application/json";
                var resp = new ApiResponse<PageResult<AddressResponse>>
                {
                    Code = StatusCodes.Status403Forbidden,
                    Success = false,
                    Message = "You do not have permission to perform this function.",
                    Data = null,
                };
                await context.Response.WriteAsJsonAsync(resp);
                return;
            }

            // tất cả các trường hợp khác (không token / token sai) → default (401)
            await _defaultHandler.HandleAsync(next, context, policy, authorizeResult);
        }
    }
}

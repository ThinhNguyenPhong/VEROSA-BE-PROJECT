using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VEROSA.Common.Models.Request;
using VEROSA.Common.Models.Response;

namespace VEROSA.BussinessLogicLayer.Services.Auth
{
    public interface IAuthService
    {
        Task<AuthResponse> RegisterAsync(RegisterRequest dto);
        Task<AuthResponse> LoginAsync(LoginRequest dto);
    }
}

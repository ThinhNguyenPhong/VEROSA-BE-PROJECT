using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VEROSA.Common.Models.Request;
using VEROSA.Common.Models.Response;

namespace VEROSA.BussinessLogicLayer.Services.Account
{
    public interface IAccountService
    {
        Task<AccountResponse> RegisterAsync(RegisterRequest request);
        Task<AccountResponse> LoginAsync(LoginRequest request);
    }
}

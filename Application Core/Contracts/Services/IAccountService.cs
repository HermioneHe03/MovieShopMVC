using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application_Core.Models;

namespace Application_Core.Contracts.Services
{
    public interface IAccountService
    {
        Task<bool> RegisterUser(UserRegisterModel model);
        Task<UserLoginResponseModel> LoginUser(string email, string password);
        Task<UserLoginResponseModel> GetUserById(int id);
    }
}

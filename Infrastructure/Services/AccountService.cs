using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Application_Core.Contracts.Repositories;
using Application_Core.Contracts.Services;
using Application_Core.Entities;
using Application_Core.Exceptions;
using Application_Core.Models;
using Infrastructure.Repositories;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;

namespace Infrastructure.Services
{
    public class AccountService : IAccountService
    {
        public Task<UserLoginResponseModel> GetUserById(int id)
        {
            throw new NotImplementedException();
        }

        public Task<UserLoginResponseModel> LoginUser(string email, string password)
        {
            throw new NotImplementedException();
        }

        public Task<bool> RegisterUser(UserRegisterModel model)
        {
            throw new NotImplementedException();
        }
    }
}

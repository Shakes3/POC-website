using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using POC3.Application;
using POC3.Models;
using BCrypt.Net;
using Microsoft.EntityFrameworkCore;

namespace POC3.services
{
    public interface IUserService
    {
        Task<User?> GetUserByEmailAsync(string email);
        Task<bool> IsUserLockedOutAsync(User user);
        Task<int> IncrementUnsuccessfulLoginAttemptsAsync(User user);
        Task ResetUnsuccessfulLoginAttemptsAsync(User user);
        Task LockUserAccountAsync(User user);
        Task UnlockUserAccountAsync(User user);
        Task UpdateLastLoginDateTimeAsync(User user);
        Task UpdateLastPasswordChangeDateTimeAsync(User user);
    }

    

}
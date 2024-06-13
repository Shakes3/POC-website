using System;
// using System.Data.Entity;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using POC3.Models;
using POC3.Application;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;


namespace POC3.services
{
    public class UserService : IUserService
    {
        private readonly ApplicationDbContext _dbContext;

        public UserService(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<User?> GetUserByEmailAsync(string email)
        {
            return await _dbContext.Users.FirstOrDefaultAsync(u => u.Email == email);
        }
   
        public async Task<bool> IsUserLockedOutAsync(User user)
        {
            // Check if the account is already inactive
            if (user.AccountInactiveDateTime > DateTime.UtcNow)
            {
                return true;
            }

            // Check if the user has exceeded the maximum number of unsuccessful login attempts
            if (user.UnsuccessfulLoginAttempts >= 3)
            {
                // Check if the account should be locked for 5 minutes
                if (user.LastLoginDateTime.AddMinutes(5) > DateTime.UtcNow)
                {
                    user.AccountInactiveDateTime = DateTime.UtcNow.AddMinutes(5);
                    await _dbContext.SaveChangesAsync(); // Save changes to the database
                    return true;
                }
                else
                {
                    user.UnsuccessfulLoginAttempts = 0;
                    await _dbContext.SaveChangesAsync(); // Save changes to the database
                }
            }

            return false;
        }

        public async Task<int> IncrementUnsuccessfulLoginAttemptsAsync(User user)
        {
            user.UnsuccessfulLoginAttempts++;
            _dbContext.Users.Update(user);
            await _dbContext.SaveChangesAsync(); // Save changes to the database
            return user.UnsuccessfulLoginAttempts;
        }

        public async Task ResetUnsuccessfulLoginAttemptsAsync(User user)
        {
            user.UnsuccessfulLoginAttempts = 0;
            await _dbContext.SaveChangesAsync(); // Save changes to the database

        }


        public async Task UnlockUserAccountAsync(User user)
        {
            user.IsActive = false;
            user.AccountInactiveDateTime = DateTime.MaxValue;
            await _dbContext.SaveChangesAsync(); // Save changes to the database
        }

        public async Task UpdateLastLoginDateTimeAsync(User user)
        {
            user.LastLoginDateTime = DateTime.UtcNow;
            await _dbContext.SaveChangesAsync(); // Save changes to the database
        }

        public async Task UpdateLastPasswordChangeDateTimeAsync(User user)
        {
            user.LastPasswordChangeDateTime = DateTime.UtcNow;
            await _dbContext.SaveChangesAsync();
        }

        public async Task LockUserAccountAsync(User user)
        {
            // throw new NotImplementedException();
            user.IsActive = false;
            user.AccountInactiveDateTime = DateTime.UtcNow.AddHours(5); // Lock the account for 5 hours
            await _dbContext.SaveChangesAsync(); // Save changes to the database
        }
    }
}
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using POC3.Models;
using POC3.services;
using POC3.Application;
using System.Security.Cryptography;


namespace POC3.Controllers
{
    // [Route("[controller]")]
    public class HomeController : Controller
    {
        private readonly IUserService _accountService;
        private readonly ApplicationDbContext _dbContext;

        public HomeController(IUserService accountService, ApplicationDbContext dbContext)
        {
            _accountService = accountService;
            _dbContext = dbContext;
        }

        [HttpGet]
        public IActionResult Login()
        {
            var model = new LoginViewModel();
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await _accountService.GetUserByEmailAsync(model.Email);
            // var userpassword = model.Password;   //wrong 
            if (user != null && model.Password != null)
            {
                var pw = ValidatePassword(model.Password, user.Password, user.Salt);

                // things Todo - to check the login password we need to salt hash the password which is saved in the data to match it 
                // or we can hash salt the incoming password and match (maybe)

                if (user == null)
                {
                    ModelState.AddModelError("", "Invalid email or password.");
                    return View(model);
                }


                if (await _accountService.IsUserLockedOutAsync(user))
                {
                    ModelState.AddModelError("", "Account locked. Try again later.");
                    return View(model);
                }

                // calling the validatepassword method here to hash the password

                if (ValidatePassword(model.Password, user.Password, user.Salt))
                {
                    await _accountService.ResetUnsuccessfulLoginAttemptsAsync(user);
                    await _accountService.UpdateLastLoginDateTimeAsync(user);
                    return RedirectToAction("Home", "Main");
                }
                else
                {
                    await _accountService.IncrementUnsuccessfulLoginAttemptsAsync(user);

                    if (user.UnsuccessfulLoginAttempts >= 3)
                    {
                        await _accountService.LockUserAccountAsync(user);
                        ModelState.AddModelError("", "Account locked. Try again later.");
                    }
                    else
                    {
                        ModelState.AddModelError("", "Invalid email or password.");
                    }

                    return View(model);
                }

            }
            ModelState.AddModelError("", "Invalid email or password.");
            return View(model);

        }


        // creating a method to use above
        private bool ValidatePassword(string password, string hash, string salt)
        {
            // Console.WriteLine(password);
            // password=password.ToLower();
            // Console.WriteLine(hash);

            byte[] storedSaltBytes = Convert.FromBase64String(salt);
            // byte[] storedHashBytes = Convert.FromBase64String(hash); this was wrong
            // var storedHashBytes = Convert.FromBase64String(hash);sample
            // var storedSaltBytes = Convert.FromBase64String(salt);sample
            // Console.WriteLine(storedHashBytes[1]);
            Console.WriteLine("Hello");

            // using (var hmac = new System.Security.Cryptography.HMACSHA512(storedSaltBytes))
            // {
            //     var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));

            //     // Console.WriteLine("stored hash  " + BitConverter.ToString(storedHashBytes));
            //     // Console.WriteLine("computed hash  " + BitConverter.ToString(computedHash));

            //     // Console.WriteLine(computedHash[1]);

            //     for (int i = 0; i < computedHash.Length; i++)
            //     {
            //         if (computedHash[i] != storedHashBytes[i])
            //         {
            //             return false;
            //         }
            //     }


            // var IsVerified = Crypto.VerifyHashedPassword(computedHash, storedHashBytes);

            // Verify obj = new Verify();
            // obj.IsVerified = IsVerified;
            // return false;

            var comingpassword = Convert.ToBase64String(Encoding.UTF8.GetBytes(password));
            var savesalt = Convert.ToBase64String(Encoding.UTF8.GetBytes(salt));
            var passwordsalt = savesalt + comingpassword;
            for (int i = 0; i < passwordsalt.Length; i++)
            {
                if (passwordsalt[i] != hash[i])
                {
                    return false;
                }
            }

            return true;
        }

        // Registration action method

        [HttpGet]
        public IActionResult Register()
        {
            // var model = new User();
            return View();
        }

        // POST: /Registration/Register
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(User model)
        {
            // if (!ModelState.IsValid)
            // {
            //     return View(model);
            // }

            // Check if the email is already registered
            if (await _accountService.GetUserByEmailAsync(model.Email) != null)
            {
                ModelState.AddModelError("EmailExists", "Email is already registered.");
                return View(model);
            }
            (string hashedPassword, string salt) = HashPassword(model.Password);

            var user = new User
            {
                Email = model.Email,
                Password = hashedPassword, // You'll need to hash the password before storing it in the database
                Name = model.Name,
                Salt = salt, // Generate a unique salt for each user
                LastLoginDateTime = DateTime.UtcNow,
                LastPasswordChangeDateTime = DateTime.UtcNow,
                IsActive = true,
                UnsuccessfulLoginAttempts = 0,
                AccountCreationDateTime = DateTime.UtcNow,
                AccountInactiveDateTime = DateTime.MinValue // Set to MinValue to indicate the account is active
            };

            // Save the user to the database
            // await _userService.RegisterUserAsync(user);
            _dbContext.Users.Add(user);
            await _dbContext.SaveChangesAsync();

            // Redirect to login page or any other page
            return RedirectToAction("Login", "Home");
        }

        // Method to hash the password and generate a salt
        private (string hashedPassword, string salt) HashPassword(string password)
        {
            // Generate a salt
            byte[] saltBytes = new byte[64]; // Generate a 32-byte salt
            using (var rng = new RNGCryptoServiceProvider())
            {
                rng.GetBytes(saltBytes);
            }
            string salt = Convert.ToBase64String(saltBytes);

            // Hash the password with the salt
            string hashedPassword;

            //method 1 wrong method
            // using (var hmac = new HMACSHA512(Encoding.UTF8.GetBytes(salt)))
            // {
            //     byte[] passwordBytes = Encoding.UTF8.GetBytes(password);
            //     byte[] hashBytes = hmac.ComputeHash(passwordBytes);
            //     hashedPassword = Convert.ToBase64String(hashBytes);
            // }

            //method 2 
            var esalt = Convert.ToBase64String(Encoding.UTF8.GetBytes(salt));
            var epassword = Convert.ToBase64String(Encoding.UTF8.GetBytes(password));
            hashedPassword = esalt + epassword;

            return (hashedPassword, salt);
        }
        // another action

        [HttpGet]
        public IActionResult Home()
        {
            return View();
        }

    }

}


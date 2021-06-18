using Delivery.API.Data;
using Delivery.API.Services;
using Delivery.Models;
using Delivery.Models.ModelView;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Delivery.API.Repository.Account
{
    public class AccountRepository : IAccountRepository
    {
        private readonly ApplicationDb _db;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<ApplicationRole> _roleManager;

        public AccountRepository(ApplicationDb db, UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager, RoleManager<ApplicationRole> roleManager)
        {
            _db = db;
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            AppServices.ErrorMessage = null;
        }

        public async Task<string> GetRoleNameByUserId(string id)
        {
            var userRole = await _db.UserRoles.FirstOrDefaultAsync(x => x.UserId == id);
            return await _db.Roles.Where(x => x.Id == userRole.RoleId).Select(x => x.Name).FirstOrDefaultAsync();
        }

        public async Task IsInRoleAsync(ApplicationUser user)
        {
            if (await _roleManager.RoleExistsAsync("User"))
            {
                if (!await _userManager.IsInRoleAsync(user, "User") && !await _userManager.IsInRoleAsync(user, "Admin"))
                {
                    await _userManager.AddToRoleAsync(user, "User");
                }
            }
        }

        public async Task<ApplicationUser> LoginAsync(LoginModel login)
        {
            await CheckOrCreateRoles();

            var user = await _userManager.FindByEmailAsync(login.Email);
            if (user == null)
            {
                AppServices.ErrorMessage = "Account not existes";
                return null;
            }

            var result = await _signInManager.PasswordSignInAsync(user, login.Password, true, false);
            if (result.Succeeded)
            {
                return user;
            }
            else
            {
                if (result.IsLockedOut)
                {
                    AppServices.ErrorMessage = "Account is locked";
                }
                if (result.IsNotAllowed)
                {
                    AppServices.ErrorMessage = "No Authentication for current user";
                }
                AppServices.ErrorMessage = "Account not existes";
            }

            return null;
        }

        public async Task<ApplicationUser> RegisterAsync(RegisterModel register)
        {
            await CheckOrCreateRoles();

            if (await _db.Users.AnyAsync(x => x.UserName == register.UserName))
            {
                AppServices.ErrorMessage = "UserName Exists try anotehr one...";
                return null;
            }
            if (await _db.Users.AnyAsync(x => x.Email == register.Email))
            {
                AppServices.ErrorMessage = "Email address Exists try anotehr one...";
                return null;
            }

            var user = new ApplicationUser
            {
                Email = register.Email,
                UserName = register.UserName,
                DateJoin = DateTime.Now,
                EmailConfirmed = true
            };

            var result = await _userManager.CreateAsync(user, register.Password);
            if (result.Succeeded)
            {
                return user;
            }
            foreach (var item in result.Errors)
            {
                var error = item.Description;
                if (error != null)
                    AppServices.ErrorMessage = error;
            }

            return null;
        }

        private async Task CheckOrCreateRoles()
        {
            var admin = await _roleManager.FindByNameAsync("Admin");
            if (admin == null)
            {
                var role = new ApplicationRole
                {
                    Name = "Admin"
                };
                await _roleManager.CreateAsync(role);
            }

            var user = await _roleManager.FindByNameAsync("User");
            if (user == null)
            {
                var role = new ApplicationRole
                {
                    Name = "User"
                };
                await _roleManager.CreateAsync(role);
            }
        }
    }
}

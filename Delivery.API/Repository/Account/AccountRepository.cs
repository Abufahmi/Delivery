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

        public AccountRepository(ApplicationDb db, UserManager<ApplicationUser> userManager)
        {
            _db = db;
            _userManager = userManager;
            AppServices.ErrorMessage = null;
        }

        public async Task<ApplicationUser> RegisterAsync(RegisterModel register)
        {
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
    }
}

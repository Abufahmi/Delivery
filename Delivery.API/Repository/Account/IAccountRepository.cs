using Delivery.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Delivery.API.Repository.Account
{
    public interface IAccountRepository
    {
        Task<ApplicationUser> RegisterAsync();
    }
}

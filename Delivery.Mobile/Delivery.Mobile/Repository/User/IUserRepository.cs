using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Delivery.Mobile.Repository.User
{
    public interface IUserRepository
    {
        Task<bool> RegisterAsync(string username, string email, string password, string passwordConfirm);
    }
}

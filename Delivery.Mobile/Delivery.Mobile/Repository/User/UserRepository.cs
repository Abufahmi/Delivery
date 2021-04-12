using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Delivery.Mobile.Repository.User
{
    public class UserRepository : IUserRepository
    {
        public async Task<bool> RegisterAsync(string username, string email, string password, string passwordConfirm)
        {
            throw new NotImplementedException();
        }
    }
}

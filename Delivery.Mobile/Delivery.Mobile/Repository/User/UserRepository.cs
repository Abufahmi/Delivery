using Delivery.Mobile.Services;
using Delivery.Models.ModelView;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Delivery.Mobile.Repository.User
{
    public class UserRepository : IUserRepository
    {
       private HttpClientHandler handler;

        public UserRepository()
        {
            AppServices.Error = null;
            handler = new HttpClientHandler();
        }

        public async Task<bool> RegisterAsync(string username, string email, string password, string passwordConfirm)
        {
            handler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };
            var client = new HttpClient(handler);
            var model = new RegisterModel
            {
                UserName = username,
                Email = email,
                Password = password,
                PasswordConfirm = password
            };

            var json = JsonConvert.SerializeObject(model);
            HttpContent httpContent = new StringContent(json);
            httpContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            try
            {
                var result = await client.PostAsync($"{App.BaseApi}api/Account/Register", httpContent);
                if (result.IsSuccessStatusCode)
                {
                    return true;
                }

                AppServices.Error = await result.Content.ReadAsStringAsync();
            }
            catch (Exception ex)
            {
                AppServices.Error = ex.Message;
            }

            return false;
        }
    }
}

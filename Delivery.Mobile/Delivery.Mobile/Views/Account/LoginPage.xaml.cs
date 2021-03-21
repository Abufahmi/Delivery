using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Delivery.Mobile.Views.Account
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoginPage : ContentPage
    {
        public LoginPage()
        {
            InitializeComponent();

            try
            {
                var assembly = typeof(LoginPage);
                logImage.Source = ImageSource.FromResource("Delivery.Mobile.Assets.images.Login.png", assembly);
            }
            catch (Exception)
            {
                throw;
            }
        }

        private async void Button_Clicked(object sender, EventArgs e)
        {
            var email = txtEmail.Text;
            var pass = txtPassword.Text;
            if (string.IsNullOrEmpty(email))
            {
                await DisplayAlert("Login", "Email address required", "OK");
                return;
            }
            if (string.IsNullOrEmpty(pass))
            {
                await DisplayAlert("Login", "The password is required", "OK");
                return;
            }

            var regex = new Regex(@"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$");
            if (!regex.IsMatch(email))
            {
                await DisplayAlert("Login", "Email address not valid", "OK");
                return;
            }

            await DisplayAlert("Login", email, "OK");
        }
    }
}
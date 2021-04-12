using Delivery.Mobile.Repository.User;
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
    public partial class RegisterPage : ContentPage
    {
        public RegisterPage()
        {
            InitializeComponent();
        }

        private async void btnReister_Clicked(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtUserName.Text) || string.IsNullOrEmpty(txtEmail.Text) || string.IsNullOrEmpty(txtPassword.Text)
                || string.IsNullOrEmpty(txtPasswordConfirm.Text))
            {
                return;
            }

            if (txtPassword.Text != txtPasswordConfirm.Text)
            {
                await DisplayAlert("Register", "Password and password confirm not equals", "Ok");
                return;
            }

            if(txtPassword.Text.Length < 6)
            {
                await DisplayAlert("Register", "password must be at least 6 char", "OK");
                return;
            }

            var regex = new Regex(@"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$");
            if (!regex.IsMatch(txtEmail.Text))
            {
                await DisplayAlert("Register", "Email address not valid", "OK");
                return;
            }

            IUserRepository repository = new UserRepository();
            var register = await repository.RegisterAsync(txtUserName.Text, txtEmail.Text, txtPassword.Text, txtPasswordConfirm.Text);
        }

        private void btnLogin_Clicked(object sender, EventArgs e)
        {

        }
    }
}
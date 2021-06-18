using Delivery.Mobile.Repository.User;
using Delivery.Mobile.Services;
using Delivery.Mobile.Views;
using Delivery.Mobile.Views.Account;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using Xamarin.Forms;

namespace Delivery.Mobile.ViewModels.Accounts
{
    public class LoginViewModel : BaseModelView
    {
        private string email;
        private string password;

        public LoginViewModel()
        {
            this.EmailMessage = "Email address is required";
            this.RegisterCommand = new Command(RegisterClick);
            this.LoginCommand = new Command(LoginClick);
        }

        public Command RegisterCommand { get; set; }

        public Command LoginCommand { get; set; }

        public string Email
        {
            get
            {
                return this.email;
            }
            set
            {
                if (this.email != value)
                {
                    this.email = value;
                    this.NotifyPropertyChanged();
                    if (string.IsNullOrWhiteSpace(email))
                    {
                        this.IsEmailValid = true;
                        this.NotifyPropertyChanged("IsEmailValid");
                        this.EmailMessage = "Email address is required";
                        this.NotifyPropertyChanged("EmailMessage");
                    }
                    else
                    {
                        if (ValidateEmail())
                        {
                            this.IsEmailValid = false;
                            this.NotifyPropertyChanged("IsEmailValid");
                        }
                        else
                        {
                            this.IsEmailValid = true;
                            this.NotifyPropertyChanged("IsEmailValid");
                            this.EmailMessage = "Email address not valid";
                            this.NotifyPropertyChanged("EmailMessage");
                        }
                    }
                }
            }
        }


        public string Password
        {
            get
            {
                return this.password;
            }
            set
            {
                if (this.password != value)
                {
                    this.password = value;
                    this.NotifyPropertyChanged();
                    if (string.IsNullOrWhiteSpace(password))
                    {
                        this.IsPasswordValid = true;
                        this.NotifyPropertyChanged("IsPasswordValid");
                    }
                    else
                    {
                        this.IsPasswordValid = false;
                        this.NotifyPropertyChanged("IsPasswordValid");
                    }
                }
            }
        }


        public bool IsEmailValid { get; set; }

        public bool IsPasswordValid { get; set; }

        public string EmailMessage { get; set; }

        private bool ValidateEmail()
        {
            var regex = new Regex(@"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$");
            if (!regex.IsMatch(Email))
            {
                return false;
            }
            return true;
        }

        private async void LoginClick(object obj)
        {
            if (string.IsNullOrWhiteSpace(this.Email) || string.IsNullOrWhiteSpace(this.Password) || !ValidateEmail())
            {
                return;
            }

            IUserRepository repository = new UserRepository();
            var login = await repository.LoginAsync(Email, Password);
            if (login)
            {
                await App.Current.MainPage.Navigation.PushAsync(new HomePage());
            }
            else
            {
                if (AppServices.Error != null)
                    await App.Current.MainPage.DisplayAlert("Login", AppServices.Error, "Ok");
            }
        }

        private async void RegisterClick(object obj)
        {
            await App.Current.MainPage.Navigation.PushAsync(new RegisterPage());
        }
    }
}

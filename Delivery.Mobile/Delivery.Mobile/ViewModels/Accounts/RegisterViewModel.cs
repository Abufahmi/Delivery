using Delivery.Mobile.Repository.User;
using Delivery.Mobile.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using Xamarin.Forms;

namespace Delivery.Mobile.ViewModels.Accounts
{
    public class RegisterViewModel : BaseModelView
    {
        private string userName;
        private string email;
        private string password;
        private string passwordConfirm;

        public RegisterViewModel()
        {
            this.RegisterCommand = new Command(RegisterClick);
            this.LoginCommand = new Command(LoginClick);

        }


        public Command RegisterCommand { get; set; }

        public Command LoginCommand { get; set; }


        public string UserName
        {
            get
            {
                return this.userName;
            }
            set
            {
                if (this.userName != value)
                {
                    this.userName = value;
                    this.NotifyPropertyChanged();
                    if (string.IsNullOrWhiteSpace(userName))
                    {
                        this.IsUserNameValid = true;
                        this.NotifyPropertyChanged("IsUserNameValid");
                    }
                    else
                    {
                        this.IsUserNameValid = false;
                        this.NotifyPropertyChanged("IsUserNameValid");
                    }
                }
            }
        }

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
                    }
                    else
                    {
                        this.IsEmailValid = false;
                        this.NotifyPropertyChanged("IsEmailValid");
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

        public string PasswordConfirm
        {
            get
            {
                return this.passwordConfirm;
            }
            set
            {
                if (this.passwordConfirm != value)
                {
                    this.passwordConfirm = value;
                    this.NotifyPropertyChanged();
                    if (string.IsNullOrWhiteSpace(passwordConfirm))
                    {
                        this.IsPasswordConfirmValid = true;
                        this.NotifyPropertyChanged("IsPasswordConfirmValid");
                    }
                    else
                    {
                        this.IsPasswordConfirmValid = false;
                        this.NotifyPropertyChanged("IsPasswordConfirmValid");
                    }
                }
            }
        }

        public bool IsUserNameValid { get; set; }

        public bool IsEmailValid { get; set; }

        public bool IsPasswordValid { get; set; }

        public bool IsPasswordConfirmValid { get; set; }


        private void LoginClick(object obj)
        {

        }

        private async void RegisterClick(object obj)
        {
            if (string.IsNullOrEmpty(UserName) || string.IsNullOrEmpty(Email) || string.IsNullOrEmpty(Password)
              || string.IsNullOrEmpty(PasswordConfirm))
            {
                return;
            }

            if (Password != PasswordConfirm)
            {
                await App.Current.MainPage.DisplayAlert("Register", "Password and password confirm not equals", "Ok");
                return;
            }

            if (Password.Length < 6)
            {
                await App.Current.MainPage.DisplayAlert("Register", "password must be at least 6 char", "OK");
                return;
            }

            var regex = new Regex(@"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$");
            if (!regex.IsMatch(Email))
            {
                await App.Current.MainPage.DisplayAlert("Register", "Email address not valid", "OK");
                return;
            }

            IUserRepository repository = new UserRepository();
            var register = await repository.RegisterAsync(UserName, Email, Password, PasswordConfirm);
            if (!register)
            {
                if (AppServices.Error != null)
                {
                    await App.Current.MainPage.DisplayAlert("Error", AppServices.Error, "OK");
                }
                return;
            }
            await App.Current.MainPage.DisplayAlert("Register", "Registration Success", "OK");
        }
    }
}

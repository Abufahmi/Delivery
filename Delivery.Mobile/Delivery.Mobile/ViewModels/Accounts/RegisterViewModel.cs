using Delivery.Mobile.Repository.User;
using Delivery.Mobile.Services;
using Delivery.Mobile.Views.Account;
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
            this.EmailMessage = "Email address is required";
            this.PassMessage = "Password is required";
            this.PassConfirmMessage = "Password Confirm is required";
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
                        this.PassMessage = "Password is required";
                        this.NotifyPropertyChanged("PassMessage");
                    }
                    else
                    {
                        if (ValidatePassword())
                        {
                            this.IsPasswordValid = false;
                            this.NotifyPropertyChanged("IsPasswordValid");
                        }
                        else
                        {
                            this.PassMessage = "Password must be at least 6 character and contains one (digit - lowerCase - UpperCase - Uniqe Char)";
                            this.NotifyPropertyChanged("PassMessage");
                            this.IsPasswordValid = true;
                            this.NotifyPropertyChanged("IsPasswordValid");
                        }

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
                        this.PassConfirmMessage = "Password Confirm is required";
                        this.NotifyPropertyChanged("PassConfirmMessage");
                    }
                    else
                    {
                        if (IsPasswordMatch())
                        {
                            this.IsPasswordConfirmValid = false;
                            this.NotifyPropertyChanged("IsPasswordConfirmValid");
                        }
                        else
                        {
                            this.IsPasswordConfirmValid = true;
                            this.NotifyPropertyChanged("IsPasswordConfirmValid");
                            this.PassConfirmMessage = "Password and Password Confirm not match";
                            this.NotifyPropertyChanged("PassConfirmMessage");
                        }

                    }
                }
            }
        }

        public bool IsUserNameValid { get; set; }

        public bool IsEmailValid { get; set; }

        public bool IsPasswordValid { get; set; }

        public bool IsPasswordConfirmValid { get; set; }

        public string EmailMessage { get; set; }

        public string PassMessage { get; set; }

        public string PassConfirmMessage { get; set; }


        private async void LoginClick(object obj)
        {
            await App.Current.MainPage.Navigation.PushAsync(new LoginPage());
        }

        private async void RegisterClick(object obj)
        {
            if (!ValidateEntry())
            {
                return;
            }

            if (!ValidateEmail())
            {
                return;
            }

            if (!IsPasswordMatch())
            {
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

        private bool ValidateEntry()
        {
            var i = 0;
            if (string.IsNullOrWhiteSpace(UserName))
            {
                this.IsUserNameValid = true;
                this.NotifyPropertyChanged("IsUserNameValid");
                i++;
            }
            else
            {
                this.IsUserNameValid = false;
                this.NotifyPropertyChanged("IsUserNameValid");
            }

            if (string.IsNullOrWhiteSpace(Email))
            {
                this.IsEmailValid = true;
                this.NotifyPropertyChanged("IsEmailValid");
                i++;
            }
            else
            {
                this.IsEmailValid = false;
                this.NotifyPropertyChanged("IsEmailValid");
            }

            if (string.IsNullOrWhiteSpace(Password))
            {
                this.IsPasswordValid = true;
                this.NotifyPropertyChanged("IsPasswordValid");
                i++;
            }
            else
            {
                this.IsPasswordValid = false;
                this.NotifyPropertyChanged("IsPasswordValid");
            }

            if (string.IsNullOrWhiteSpace(PasswordConfirm))
            {
                this.IsPasswordConfirmValid = true;
                this.NotifyPropertyChanged("IsPasswordConfirmValid");
                i++;
            }
            else
            {
                this.IsPasswordConfirmValid = false;
                this.NotifyPropertyChanged("IsPasswordConfirmValid");
            }

            if (i > 0)
            {
                return false;
            }

            return true;
        }

        private bool ValidateEmail()
        {
            var regex = new Regex(@"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$");
            if (!regex.IsMatch(Email))
            {
                return false;
            }
            return true;
        }

        private bool ValidatePassword()
        {
            var regex = new Regex(@"^(?=.*\d)(?=.*[a-z])(?=.*[A-Z])(?=.*[a-zA-Z]).{6,}$");
            if (!regex.IsMatch(Password))
            {
                return false;
            }
            return true;
        }

        private bool IsPasswordMatch()
        {
            if (Password != PasswordConfirm)
            {
                return false;
            }
            return true;
        }
    }
}

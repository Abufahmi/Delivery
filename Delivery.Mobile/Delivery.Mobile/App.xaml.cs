using Delivery.Mobile.Views.Account;
using System;
using Xamarin.Forms;
using Xamarin.Essentials;
using Xamarin.Forms.Xaml;
using Delivery.Mobile.Views;

[assembly: ExportFont("Font Awesome 5 Free-Solid-900.otf", Alias = "FontAwesome")]
namespace Delivery.Mobile
{
    public partial class App : Application
    {
        public static string BaseApi { get; set; }
        public static string Email { get; set; }
        public static string Password { get; set; }
        public static string Token { get; set; }
        public static string UserName { get; set; }
        public static string Role { get; set; }
        public static bool IsLoged { get; set; }


        public App()
        {
            InitializeComponent();

            if (DeviceInfo.Platform == DevicePlatform.UWP)
            {
                BaseApi = "https://localhost:44394/";
            }
            else
            {
                BaseApi = "https://192.168.1.142:436/";
            }

            CheckUser();

            MainPage = new NavigationPage(new HomePage());
        }

        private void CheckUser()
        {
            var token = Plugin.Settings.CrossSettings.Current.GetValueOrDefault("token", "");
            var name = Plugin.Settings.CrossSettings.Current.GetValueOrDefault("username", "");
            var role = Plugin.Settings.CrossSettings.Current.GetValueOrDefault("role", "");
            var pass = Plugin.Settings.CrossSettings.Current.GetValueOrDefault("pass", "");
            var email = Plugin.Settings.CrossSettings.Current.GetValueOrDefault("email", "");

            Email = email;
            Password = pass;
            Token = token;
            UserName = name;
            Role = role;

            if (!string.IsNullOrEmpty(token) && !string.IsNullOrEmpty(role) && !string.IsNullOrEmpty(email) &&
                !string.IsNullOrEmpty(name) && !string.IsNullOrEmpty(pass))
            {
                IsLoged = true;
            }
            else
            {
                IsLoged = false;
            }
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}

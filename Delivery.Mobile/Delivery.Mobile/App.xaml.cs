using Delivery.Mobile.Views.Account;
using System;
using Xamarin.Forms;
using Xamarin.Essentials;
using Xamarin.Forms.Xaml;

namespace Delivery.Mobile
{
    public partial class App : Application
    {
        public static string BaseApi { get; set; }

        public App()
        {
            InitializeComponent();

            MainPage = new NavigationPage(new LoginPage());
            if (DeviceInfo.Platform == DevicePlatform.UWP)
            {
                BaseApi = "https://localhost:44394/";
            }
            else
            {
                BaseApi = "https://192.168.1.142:436/";
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

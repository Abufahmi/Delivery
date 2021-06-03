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

            //try
            //{
            //    var assembly = typeof(LoginPage);
            //    logImage.Source = ImageSource.FromResource("Delivery.Mobile.Assets.images.Login.png", assembly);
            //}
            //catch (Exception)
            //{
            //    throw;
            //}
        }
    }
}
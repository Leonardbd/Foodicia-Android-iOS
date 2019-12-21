using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Microsoft.AppCenter;
using Microsoft.AppCenter.Push;

namespace CaptoApplication
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            

            MainPage = new NavigationPage(new MainPage());
            BindingContext = new IngredientsViewModel();
        }

        protected override void OnStart()
        {
            AppCenter.Start("b7186a45-3a8b-40df-b420-5bdb2dbc6a66", typeof(Push));
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}

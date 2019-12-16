using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CaptoApplication
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            

            MainPage = new MainPage();
            BindingContext = new IngredientsViewModel();
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

using TimeDisplay.Views.Pages;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xamarin.Essentials;
using System.Linq;
using TimeDisplay.Resources.Theming;

namespace TimeDisplay
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            ThemeManager.Init(this);
            MainPage = new DisplayAll();
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

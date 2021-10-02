using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeDisplay.Resources.Localization;
using TimeDisplay.Services;
using TimeDisplay.ViewModels;
using TimeDisplay.Views.Views;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TimeDisplay.Views.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AppShell : Shell
    {
        private ClockDateTimeUpdater clockUpdater;

        public AppShell()
        {
            DigitalClock = new ClockViewModel { Name = AppLocalization.ShellHeaderTimeName, TimeZoneDifferenceToUTC = DateTime.Now - DateTime.UtcNow};
            clockUpdater = new ClockDateTimeUpdater(100, DigitalClock);
            InitializeComponent();
            ShellHeaderDigitalClock.BindingContext = DigitalClock;
        }

        private ClockViewModel DigitalClock { get; set; }

    }
}
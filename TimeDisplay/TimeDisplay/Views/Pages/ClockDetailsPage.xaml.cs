using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeDisplay.Data;
using TimeDisplay.Resources.Theming;
using TimeDisplay.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TimeDisplay.Views.Pages
{
    /// <summary>
    /// Creates an Clock Details page for viewing, inserting or editing a clock <br/>
    /// Accepts following shell (navigation) query parameters: <br/>
    /// • <b>'id'</b>: values: <b>int</b>, <b>default</b>: -1 (inserting) <br/>
    /// </summary>
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ClockDetailsPage : ContentPage
    {
        public ClockDetailsPage()
        {
            InitializeComponent();
            var repository = (IClockRepository)Data.RepositoryFactory.GetRepository<int, Models.ClockModel>();
            BindingContext = new ClockDetailsViewModel(repository);
        }

        private void Button_Clicked(object sender, EventArgs e)
        {
            ThemeManager.CurrentTheme = ThemeManager.CurrentTheme == ColorPalettes.ColorScheme.Dark ? ColorPalettes.ColorScheme.Light : ColorPalettes.ColorScheme.Dark;
        }
    }

    public class TimeToSignedHourMinutesConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value is TimeSpan span
                ? (span <= TimeSpan.FromMinutes(-1) ? "-" : "") + span.ToString(@"h\:mm", CultureInfo.InvariantCulture)
                : null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
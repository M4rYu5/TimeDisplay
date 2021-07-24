using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TimeDisplay.Views.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DigitalClock : ContentView
    {
        public DigitalClock()
        {
            InitializeComponent();
        }
    }

    public class DateTimeToTimeComponentConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var dateTime = (DateTime)value;
            var param = (string)parameter;
            if (param.StartsWith("hour"))
                return dateTime.Hour.ToString();
            if (param.StartsWith("min"))
                return dateTime.Minute.ToString();
            if (param.StartsWith("sec"))
                return dateTime.Second.ToString();

            return "";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
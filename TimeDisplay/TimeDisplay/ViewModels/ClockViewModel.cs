using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using TimeDisplay.Models;
using Xamarin.Forms;

namespace TimeDisplay.ViewModels
{
    public class ClockViewModel : BaseViewModel, IUpdateable
    {
        private string name;
        private TimeSpan timeZoneDifferenceToUTC;
        private DateTime dateTime;


        public ClockViewModel(ClockModel model = null)
        {
            model ??= new ClockModel();
            if (model == null) throw new ArgumentNullException(nameof(model));

            Name = model.Name;
            TimeZoneDifferenceToUTC = model.TimeZoneDifferenceToUTC;
        }

        public string Name { get { return name; } set { SetProperty(ref name, value); } }
        public TimeSpan TimeZoneDifferenceToUTC
        {
            get => timeZoneDifferenceToUTC; set
            {
                SetProperty(ref timeZoneDifferenceToUTC, value);
                Update();
            }
        }
        public DateTime DateTime { get => dateTime; set => SetProperty(ref dateTime, value); }


        public void Update() => DateTime = DateTime.UtcNow + TimeZoneDifferenceToUTC;
    }
}

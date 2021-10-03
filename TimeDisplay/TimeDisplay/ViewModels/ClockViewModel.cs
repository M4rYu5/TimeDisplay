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
        private int id;
        private string name;
        private TimeSpan timeZoneDifferenceToUTC;
        private DateTime dateTime;


        public int ID { get { return id; } set { SetProperty(ref id, value); } }
        public string Name { get { return name; } set { SetProperty(ref name, value); } }
        public TimeSpan TimeZoneDifferenceToUTC
        {
            get => timeZoneDifferenceToUTC;
            set
            {
                SetProperty(ref timeZoneDifferenceToUTC, value);
                Update();
            }
        }
        public DateTime DateTime { get => dateTime; private set => SetProperty(ref dateTime, value); }

        public void Update() => DateTime = DateTime.UtcNow + TimeZoneDifferenceToUTC;

        /// <summary>
        /// Generate a new ClockViewModel base on specified model <\br>
        /// Note: Those propertyes are not set by reference
        /// </summary>
        public static ClockViewModel FromModel(ClockModel model)
        {
            return new ClockViewModel()
            {
                ID = model.ID,
                Name = model.Name,
                TimeZoneDifferenceToUTC = model.TimeZoneDifferenceToUTC
            };
        }
    }
}

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using TimeDisplay.Models;
using TimeDisplay.Services;
using Xamarin.Forms;

namespace TimeDisplay.ViewModels
{
    public class ClockViewModel : BaseViewModel, IUpdateable
    {
        private int id = -1;
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
        /// Generate a new ClockViewModel from a specified model <\br>
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

    /// <summary>
    /// Stores the ClockViewModel's extensions
    /// </summary>
    public static class ClockViewModelExtensions
    {
        /// <summary>
        /// Creates a new ClockModel from a ClockViewModel <\br>
        /// </summary>
        public static ClockModel ToModel(this ClockViewModel vm)
        {
            return new ClockModel()
            {
                ID = vm.ID,
                Name = vm.Name,
                TimeZoneDifferenceToUTC = vm.TimeZoneDifferenceToUTC
            };
        }
    }
}

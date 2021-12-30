using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using TimeDisplay.Data;
using TimeDisplay.Models;
using Xamarin.Forms;

namespace TimeDisplay.ViewModels
{

    /// <summary>
    /// This ViewModel is accepting 'id' parameter
    /// </summary>
    public class ClockDetailsViewModel : BaseViewModel, IQueryAttributable
    {
        private IClockRepository repository;
        private ClockViewModel originalClockViewModel;
        private bool isBusy;
        private string repositoryIdNotFoundError;
        private bool modelChanged;
        private readonly ClockModel currentClock = new ClockModel();


        public ClockDetailsViewModel(IClockRepository repository)
        {
            this.repository = repository;
            PropertyChanged += ClockDetailsViewModel_PropertyChanged;
        }

        private void ClockDetailsViewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (IsClockModleProperty(e.PropertyName))
            {
                ClockModelChanged = CheckClockModelChanged();
            }
                
        }

        public bool IsBusy { get => isBusy; set => SetProperty(ref isBusy, value); }
        public string RepositoryIdNotFoundError { get => repositoryIdNotFoundError; set => SetProperty(ref repositoryIdNotFoundError, value); }
        public bool ClockModelChanged { get => modelChanged; set => SetProperty(ref modelChanged, value); }


        public int ID
        {
            get => currentClock.ID; 
            set
            {
                currentClock.ID = value;
                OnPropertyChanged(nameof(ID));
            }
        }
        public TimeSpan TimeZoneDifferenceToUTC { get => currentClock.TimeZoneDifferenceToUTC;
            set
            {
                currentClock.TimeZoneDifferenceToUTC = value;
                OnPropertyChanged(nameof(TimeZoneDifferenceToUTC));
            }
        }
        public string Name { get => currentClock.Name;
            set
            {
                currentClock.Name = value;
                OnPropertyChanged(nameof(Name));
            }
        }


        public void ApplyQueryAttributes(IDictionary<string, string> query)
        {
            // process the 'id' query param; and setting the ID property
            if (query.ContainsKey("id"))
            {
                string idQueryParam = HttpUtility.UrlDecode(query["id"]);
                if (int.TryParse(idQueryParam, out int id))
                {
                    ID = id;
                    PullInfo(id);
                }
                else
                    throw new ArgumentException("The shell query parameter 'id' cannot be converted into an int value");
            }

        }

        private void PullInfo(int id)
        {
            int localID = id;
            if(localID < 0)
            {
                originalClockViewModel = new ClockViewModel();
                ID = localID;
                return;
            }

            IClockRepository localClockRepository = repository;
            IsBusy = true;
            Task.Run(async () =>
            {
                var item = await localClockRepository.Get(localID);
                if (item == null)
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        RepositoryIdNotFoundError = Resources.Localization.AppLocalization.ClockDetails_ClockIdNotFound;
                        IsBusy = false;
                    });
                else
                {
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        var vm = ClockViewModel.FromModel(item);
                        originalClockViewModel = vm;
                        ID = vm.ID;
                        Name = vm.Name;
                        TimeZoneDifferenceToUTC = vm.TimeZoneDifferenceToUTC;
                        IsBusy = false;
                    });
                }
            });
        }


        private bool CheckClockModelChanged()
        {
            // check state
            if (IsBusy)
                return false;
            if(originalClockViewModel == null)
                return false;

            // check changes
            if (Name != originalClockViewModel.Name)
                return true;
            if (TimeZoneDifferenceToUTC != originalClockViewModel.TimeZoneDifferenceToUTC)
                return true;

            // default
            return false;
        }

        private bool IsClockModleProperty(string propertyName)
        {
            return propertyName == nameof(Name) || propertyName == nameof(TimeZoneDifferenceToUTC) || propertyName == nameof(ID);
        }
    }
}

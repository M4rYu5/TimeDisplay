using MvvmHelpers.Commands;
using MvvmValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Windows.Input;
using TimeDisplay.Data;
using TimeDisplay.Models;
using TimeDisplay.Resources.Localization;
using Xamarin.Forms;

namespace TimeDisplay.ViewModels
{

    /// <summary>
    /// This ViewModel is accepting 'id' parameter
    /// </summary>
    public class ClockDetailsViewModel : BaseViewModel, IQueryAttributable
    {
        private readonly ClockDetailVMModel currentClock = new ClockDetailVMModel();
        private readonly ICommand goToEntry;
        private readonly ICommand submitChanges;
        private readonly IClockRepository repository;

        private ClockDetailVMModel originalClockViewModel;
        private bool isBusy;
        private string repositoryIdNotFoundError;
        private string nameError;
        private string utcStringError;
        private bool modelChanged;
        private bool canSubmitChanges;
        private Func<Task> submitAction;
        private bool actionRunning = false;

        public ClockDetailsViewModel(IClockRepository repository)
        {
            this.repository = repository;
            goToEntry = new Xamarin.Forms.Command<object>((o) =>
            {
                if (o is not Entry entry)
                    return;

                entry.Focus();
                entry.CursorPosition = entry.Text.Length;
            });
            submitChanges = new AsyncCommand(() => submitAction?.Invoke(), (obj) => CanSubmitChanges && submitAction != null && !actionRunning);
            PropertyChanged += ClockDetailsViewModel_PropertyChanged;
            InitValidation();
        }

        private void ClockDetailsViewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (IsClockModleProperty(e.PropertyName))
            {
                UpdateClockModelChanged();
                Validator.ValidateAll();
            }

        }


        public bool IsBusy { get => isBusy; set => SetProperty(ref isBusy, value); }
        public string RepositoryIdNotFoundError { get => repositoryIdNotFoundError; set => SetProperty(ref repositoryIdNotFoundError, value); }
        public string NameError { get => nameError; set => SetProperty(ref nameError, value); }
        public string UtcStringError { get => utcStringError; set => SetProperty(ref utcStringError, value); }
        public bool ClockModelChanged { get => modelChanged; set => SetProperty(ref modelChanged, value); }
        public bool CanSubmitChanges { get => canSubmitChanges; protected set => SetProperty(ref canSubmitChanges, value); }

        public ICommand GoToEntry { get => goToEntry; }
        public ICommand SubmitChanges { get => submitChanges; }


        public int ID
        {
            get => currentClock.ID;
            set
            {
                if (value != currentClock.ID)
                {
                    currentClock.ID = value;
                    OnPropertyChanged(nameof(ID));
                }
            }
        }
        public string TimeZoneDifferenceToUtcString
        {
            get => currentClock.TimeZoneDifferenceToUtcString;
            set
            {
                if (value != currentClock.TimeZoneDifferenceToUtcString)
                {
                    currentClock.TimeZoneDifferenceToUtcString = value;
                    OnPropertyChanged(nameof(TimeZoneDifferenceToUtcString));
                }
            }
        }
        public string Name
        {
            get => currentClock.Name;
            set
            {
                if (value != currentClock.Name)
                {
                    currentClock.Name = value;
                    OnPropertyChanged(nameof(Name));
                }
            }
        }




        public void ApplyQueryAttributes(IDictionary<string, string> query)
        {
            // process the 'id' query param; and setting the ID property
            int localID;
            if (query.ContainsKey("id"))
            {
                string idQueryParam = HttpUtility.UrlDecode(query["id"]);
                if (int.TryParse(idQueryParam, out int id))
                    localID = id;
                else
                    throw new ArgumentException("The shell query parameter 'id' cannot be converted into an int value");
            }
            else
                localID = -1;

            PullInfo(localID);


        }

        private void PullInfo(int id)
        {
            if (id < 0)
            {
                InitInsertMode();
                return;
            }

            IClockRepository localClockRepository = repository;
            IsBusy = true;
            Task.Run(async () =>
            {
                var item = await localClockRepository.Get(id);
                if (item == null)
                    Device.BeginInvokeOnMainThread(() => InitNotFoundMode(id));
                else
                    Device.BeginInvokeOnMainThread(() => InitEditMode(item));
            });
        }


        private void InitInsertMode()
        {
            ID = -1;
            UpdateClockModelChanged();
            submitAction = async () =>
            {
                await InsertCurrentModelInRepository();
                await Shell.Current.GoToAsync("../");
            };
        }
        
        private void InitEditMode(ClockModel item)
        {
            var vm = ClockViewModel.FromModel(item);
            originalClockViewModel = ClockDetailVMModel.FromClockViewModel(vm);
            ID = vm.ID;
            Name = vm.Name;
            TimeZoneDifferenceToUtcString = originalClockViewModel.TimeZoneDifferenceToUtcString;
            IsBusy = false;
            submitAction = async () =>
            {
                await UpdateCurrentModelInRepository();
                await Shell.Current.GoToAsync("../");
            };
        }

        private void InitNotFoundMode(int localID)
        {
            RepositoryIdNotFoundError = Resources.Localization.AppLocalization.ClockDetails_ClockIdNotFound;
            IsBusy = false;
            ID = localID;
        }

        private async Task InsertCurrentModelInRepository()
        {
            ClockModel model = currentClock;
            if (model == null || !Validator.GetResult().IsValid)
                return;

            actionRunning = true;
            await repository?.Add(model);
            actionRunning = false;
        }

        private async Task UpdateCurrentModelInRepository()
        {
            ClockModel model = currentClock;
            if (model == null || !Validator.GetResult().IsValid)
                return;

            actionRunning = true;
            await repository?.Update(model.ID, model);
            actionRunning = false;
        }

        private void UpdateClockModelChanged()
        {
            ClockModelChanged = CheckClockModelChanged();
        }

        private bool CheckClockModelChanged()
        {
            // check state
            if (IsBusy)
                return false;

            if (originalClockViewModel == null)
                return true;

            // check changes
            if (Name != originalClockViewModel.Name)
                return true;
            if (TimeZoneDifferenceToUtcString != originalClockViewModel.TimeZoneDifferenceToUtcString)
                return true;

            // default
            return false;
        }

        private bool IsClockModleProperty(string propertyName)
        {
            return propertyName == nameof(Name) || propertyName == nameof(TimeZoneDifferenceToUtcString) || propertyName == nameof(ID);
        }




        #region Validation

        protected ValidationHelper Validator { get; set; }

        protected virtual void InitValidation()
        {
            Validator = new ValidationHelper();


            Validator.AddRequiredRule(() => Name, AppLocalization.ClockDetailsPage_ErrorRequired);
            Validator.AddRule(nameof(Name), () => RuleResult.Assert(Name.Length <= 20, AppLocalization.ClockDetailsPage_ErrorNameLengthMax));


            Validator.AddRequiredRule(() => TimeZoneDifferenceToUtcString, AppLocalization.ClockDetailsPage_ErrorRequired);
            Validator.AddRule(nameof(TimeZoneDifferenceToUtcString),
                              () => RuleResult.Assert(TimeZoneDifferenceToUtcString.Contains(":"), AppLocalization.ClockDetailsPage_ErrorUTCSeparatorNeeded));
            Validator.AddRule(nameof(TimeZoneDifferenceToUtcString),
                              () =>
                              {
                                  char[] validChars = new char[] { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', ':', '-', '+', ' ' };
                                  StringBuilder builder = new StringBuilder();
                                  foreach (char c in TimeZoneDifferenceToUtcString)
                                      if (!validChars.Contains(c))
                                          builder.Append(c);
                                  var unwantedCharacters = builder.ToString();

                                  if (unwantedCharacters.Length == 0)
                                      return RuleResult.Valid();

                                  StringBuilder result = new StringBuilder();
                                  result.Append('\'');
                                  result.Append(string.Join("', '", unwantedCharacters.AsEnumerable().Distinct()));
                                  result.Append("': ");
                                  result.Append(unwantedCharacters.Length == 1 ? AppLocalization.ClockDetailsPage_InvalidTimeCharacterPostSing : AppLocalization.ClockDetailsPage_InvalidTimeCharacterPostPlur);
                                  return RuleResult.Invalid(result.ToString());
                              });
            // invalid : delimiter location
            Validator.AddRule(nameof(TimeZoneDifferenceToUtcString),
                              () =>
                              {
                                  var index = TimeZoneDifferenceToUtcString.IndexOf(":");
                                  if (index != 0 && index != TimeZoneDifferenceToUtcString.Length - 1)
                                      return RuleResult.Valid();

                                  return RuleResult.Invalid(AppLocalization.ClockDetailsPage_InvalidTimeDifference);
                              });
            // cannot convert to int
            Validator.AddRule(nameof(TimeZoneDifferenceToUtcString),
                              () =>
                              {
                                  if (!TimeZoneDifferenceToUtcString.Contains(":"))
                                      return RuleResult.Invalid(AppLocalization.ClockDetailsPage_InvalidTimeDifference);

                                  string[] splittedValues = TimeZoneDifferenceToUtcString.Split(':');
                                  if (splittedValues.Length != 2)
                                      return RuleResult.Invalid(AppLocalization.ClockDetailsPage_InvalidTimeDifference);

                                  if (!int.TryParse(splittedValues[0], out _))
                                      return RuleResult.Invalid(AppLocalization.ClockDetailsPage_InvalidTimeDifference);
                                  if (!int.TryParse(splittedValues[1], out _))
                                      return RuleResult.Invalid(AppLocalization.ClockDetailsPage_InvalidTimeDifference);

                                  return RuleResult.Valid();
                              });

            // handle errors
            Validator.ResultChanged += (sender, args) =>
            {
                if ((string)args.Target == nameof(Name))
                    NameError = args.NewResult.ErrorList.FirstOrDefault()?.ErrorText;
                if ((string)args.Target == nameof(TimeZoneDifferenceToUtcString))
                    UtcStringError = args.NewResult.ErrorList.FirstOrDefault()?.ErrorText;

                CanSubmitChanges = Validator.GetResult().IsValid;
            };
        }
        #endregion




        private class ClockDetailVMModel : ClockModel
        {
            public string TimeZoneDifferenceToUtcString { get; set; }

            public new TimeSpan TimeZoneDifferenceToUTC { get => StringToTimeSpanConverter(TimeZoneDifferenceToUtcString) ?? TimeSpan.Zero; }


            public static TimeSpan? StringToTimeSpanConverter(string time)
            {
                if (string.IsNullOrWhiteSpace(time))
                    return null;

                var localTime = time.Replace(" ", "");

                bool isNegative = localTime.StartsWith("-");
                if (isNegative)
                    localTime = localTime.Substring(1);

                if (!localTime.Contains(":"))
                    return null;

                string[] splittedValues = localTime.Split(':');
                if (splittedValues.Length != 2)
                    return null;

                if (!int.TryParse(splittedValues[0], out int hours))
                    return null;
                if (!int.TryParse(splittedValues[1], out int minutes))
                    return null;

                hours = isNegative ? -hours : hours;
                minutes = isNegative ? -minutes : minutes;

                return TimeSpan.FromHours(hours).Add(TimeSpan.FromMinutes(minutes));
            }
            public static string TimeSpanToStringConverter(TimeSpan timeSpan)
            {
                return timeSpan is TimeSpan span
                ? (span <= TimeSpan.FromMinutes(-1) ? "-" : "") + span.ToString(@"h\:mm", System.Globalization.CultureInfo.InvariantCulture)
                : null;
            }

            public static ClockDetailVMModel FromClockViewModel(ClockViewModel vm)
            {
                return new ClockDetailVMModel()
                {
                    ID = vm.ID,
                    Name = vm.Name,
                    TimeZoneDifferenceToUtcString = TimeSpanToStringConverter(vm.TimeZoneDifferenceToUTC),
                };
            }
            public static ClockModel ToClockModel(ClockDetailVMModel vm)
            {
                return new ClockDetailVMModel()
                {
                    ID = vm.ID,
                    Name = vm.Name,
                    TimeZoneDifferenceToUtcString = TimeSpanToStringConverter(vm.TimeZoneDifferenceToUTC),
                };
            }
        }
    }
}

using MvvmHelpers.Commands;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using TimeDisplay.Data;
using TimeDisplay.Data.Interfaces;
using TimeDisplay.Models;
using TimeDisplay.Services;
using Xamarin.Forms;

namespace TimeDisplay.ViewModels
{
    public class DisplayAllViewModel : BaseViewModel, IDisposable
    {
        private const int UpdateInterval = 100; //ms

        private readonly ClockDateTimeUpdater clockUpdater;
        private readonly IClockRepository repository;
        private readonly ICommand addNewDigitalClock;
        private readonly ICommand refreshCommand;

        private bool disposed;
        private bool isRefreshing;
        private ObservableCollection<ClockViewModel> clocks;




        public DisplayAllViewModel()
        {
            repository = (IClockRepository)Data.RepositoryFactory.GetRepository<int, ClockModel>();

            addNewDigitalClock = new AsyncCommand(() => Shell.Current.GoToAsync("details"));
            refreshCommand = new AsyncCommand(() => RefreshClocks(), (o) => !IsRefreshing, continueOnCapturedContext: true);

            clockUpdater = new ClockDateTimeUpdater(UpdateInterval);
        }



        public ICommand AddNewDigitalClockCommand { get => addNewDigitalClock; }
        public ICommand RefreshCommand { get => refreshCommand; }
        public bool IsRefreshing { get => isRefreshing; set => SetProperty(ref isRefreshing, value); }

        public ObservableCollection<ClockViewModel> Clocks
        {
            get => clocks;
            set
            {
                if (Clocks != null)
                    Clocks.CollectionChanged -= ClocksCollectionChanged;
                SetProperty(ref clocks, value);
                Clocks.CollectionChanged += ClocksCollectionChanged;
            }
        }



        public void Dispose()
        {
            if (!disposed)
            {
                clockUpdater.Dispose();
                disposed = true;
            }
            GC.SuppressFinalize(this);
        }




        private void ClocksCollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            var removedItems = ((IList<ClockViewModel>)e.OldItems).Where(a => !e.NewItems.Contains(a));
            var addedItems = ((IList<ClockViewModel>)e.NewItems).Where(a => !e.OldItems.Contains(a));

            ActualizeRepository(addedItems, removedItems);

            foreach (var item in addedItems)
                clockUpdater.Add(item);
        }

        // todo: Can I make this nicer?
        private void ActualizeRepository(IEnumerable<ClockViewModel> addedItems, IEnumerable<ClockViewModel> removedItems)
        {
            _ = repository.RemoveRange(removedItems.Select(x => x.ID)).GetAwaiter().GetResult();
            foreach (var item in addedItems)
                _ = repository.Add(item.ToModel()).GetAwaiter().GetResult();
        }

        private async Task RefreshClocks()
        {

            // todo: make this to work async (see Refresh)
            var elements = await repository.GetAll().ConfigureAwait(true);
            Clocks = new ObservableCollection<ClockViewModel>(elements.Select(s => ClockViewModel.FromModel(s)));

            clockUpdater.Clear();
            foreach (var item in Clocks)
                clockUpdater.Add(item);

            IsRefreshing = false;
        }

    }
}

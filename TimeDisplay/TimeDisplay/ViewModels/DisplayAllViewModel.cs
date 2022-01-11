using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
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

        private bool disposed;
        private ObservableCollection<ClockViewModel> clocks;
        private readonly ICommand addNewDigitalClock = new Command(async () => await Shell.Current.GoToAsync("details"));



        public DisplayAllViewModel()
        {
            repository = (IClockRepository)Data.RepositoryFactory.GetRepository<int, ClockModel>();
            // todo: make this to work async (see Refresh)
            Clocks = new ObservableCollection<ClockViewModel>(repository.GetAll().GetAwaiter().GetResult().Select(s => ClockViewModel.FromModel(s)));

            clockUpdater = new ClockDateTimeUpdater(UpdateInterval);
            foreach (var item in Clocks)
                clockUpdater.Add(item);
        }


        public ICommand AddNewDigitalClockCommand { get => addNewDigitalClock; }



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



        // todo: add a refresh function, something like this,
        // also should check if the server state changed
        //public void Refresh()
        //{
        //    Task.Run(async () =>
        //    {
        //        var list = await repository.GetAll();
        //        var clockViewModels = list.Select(s => ClockViewModel.FromModel(s)).ToList();
        //        Device.BeginInvokeOnMainThread(() =>
        //        {
        //            Clocks = new ReadOnlyCollection<ClockViewModel>(clockViewModels);
        //            foreach (var item in Clocks)
        //                clockUpdater.Add(item);
        //        });
        //    });
        //}




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



        public void Dispose()
        {
            if (!disposed)
            {
                clockUpdater.Dispose();
                disposed = true;
            }
            GC.SuppressFinalize(this);
        }
    }
}

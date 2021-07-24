using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using TimeDisplay.Data;
using TimeDisplay.Models;
using TimeDisplay.Services;

namespace TimeDisplay.ViewModels
{
    public class DisplayAllViewModel : BaseViewModel, IDisposable
    {
        private const int UpdateInterval = 100; //ms

        private readonly IClockRepository repository;
        private ObservableCollection<ClockViewModel> clock = new ObservableCollection<ClockViewModel>();
        private readonly ClockDateTimeUpdater clockUpdater;

        public ObservableCollection<ClockViewModel> Clocks { get => clock; set => SetProperty(ref clock, value); }

        public DisplayAllViewModel()
        {
            repository = (IClockRepository)Data.RepositoryFactory.GetRepository<ClockModel>();
            Clocks = new ObservableCollection<ClockViewModel>(repository.GetAll().GetAwaiter().GetResult().Select(s => new ClockViewModel(s)));
            Clocks.CollectionChanged += ClocksCollectionChanged;

            clockUpdater = new ClockDateTimeUpdater(UpdateInterval);
            foreach (var item in Clocks)
                clockUpdater.Add(item);
        }

        private void ClocksCollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            var removedItems = ((IList<ClockViewModel>)e.OldItems).Where(a => !e.NewItems.Contains(a));
            var addedItems = ((IList<ClockViewModel>)e.NewItems).Where(a => !e.OldItems.Contains(a));

            ActualizeRepository(addedItems, removedItems);

            foreach (var item in addedItems)
                clockUpdater.Add(item);
        }

        private void ActualizeRepository(IEnumerable<ClockViewModel> addedItems, IEnumerable<ClockViewModel> removedItems)
        {
            _ = repository.RemoveRange((IEnumerable<ClockModel>)addedItems).GetAwaiter().GetResult();
            _ = repository.AddRange((IEnumerable<ClockModel>)addedItems).GetAwaiter().GetResult();
        }

        private bool disposed;
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

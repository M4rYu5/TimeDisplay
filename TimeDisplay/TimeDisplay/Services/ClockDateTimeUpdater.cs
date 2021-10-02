using TimeDisplay.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using Xamarin.Forms;

namespace TimeDisplay.Services
{
    /// <summary>
    /// System that automatically call the Update function from IUpdate derived objects
    /// </summary>
    public class ClockDateTimeUpdater : IDisposable
    {
        private readonly List<WeakReference<IUpdateable>> clocksToUpdate = new List<WeakReference<IUpdateable>>();
        private readonly object _lock = new object();
        private readonly Timer timer;

        /// <param name="updateInterval">Specify the interval between calls, in ms</param>
        public ClockDateTimeUpdater(int updateInterval, params IUpdateable[] clocks)
        {
            timer = new Timer(Update, null, 0, updateInterval);
            foreach (var clock in clocks)
                Add(clock);
        }

        private void Update(object state)
        {
            lock (_lock)
            {
                List<WeakReference<IUpdateable>> toRemove = new List<WeakReference<IUpdateable>>();
                foreach (var clock in clocksToUpdate)
                    if (clock.TryGetTarget(out var value))
                    {
                        var local = value;
                        Device.BeginInvokeOnMainThread(() => local.Update());
                    }
                    else
                        toRemove.Add(clock);
                foreach (var item in toRemove)
                    clocksToUpdate.Remove(item);
            }
        }

        /// <summary>
        /// Set the update interval
        /// </summary>
        /// <param name="interval">Interval in milliseconds</param>
        public void SetUpdateInterval(int interval) => timer.Change(0, interval);


        /// <summary>
        /// Add a WeakReference to the specified parameter
        /// </summary>
        /// <param name="toUpdate"></param>
        public void Add(IUpdateable toUpdate)
        {
            if (toUpdate == null)
                return;

            lock (_lock)
            {
                clocksToUpdate.Add(new WeakReference<IUpdateable>(toUpdate));
            }
        }

        /// <summary>
        /// Stop updating a specific element by remving it from the internal list of WeakReferences 
        /// </summary>
        /// <param name="toUpdate">Element to be removerd</param>
        public void Remove(IUpdateable toUpdate)
        {
            if (toUpdate == null)
                return;

            lock (_lock)
            {
                clocksToUpdate.RemoveAll((c) =>
                {
                    if (c.TryGetTarget(out var item))
                        if (item == toUpdate)
                            return true;
                    return false;
                });
            }
        }

        public void Dispose()
        {
            lock (_lock)
                clocksToUpdate.Clear();
            timer.Dispose();
        }
    }
}

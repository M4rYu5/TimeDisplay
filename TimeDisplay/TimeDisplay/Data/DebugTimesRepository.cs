using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeDisplay.Data.Interfaces;
using TimeDisplay.Models;
using Xamarin.Forms;

namespace TimeDisplay.Data
{
    class DebugTimesRepository : IClockRepository
    {
        private static readonly Dictionary<int, ClockModel> debugList = new Dictionary<int, ClockModel>
        {
            {1, new ClockModel(){ID = 1, Name = "UTC", TimeZoneDifferenceToUTC = TimeSpan.Zero}},
            {2, new ClockModel(){ID = 2, Name = "UTC+1", TimeZoneDifferenceToUTC = TimeSpan.FromHours(1)}},
            {3, new ClockModel(){ID = 3, Name = "UTC+2", TimeZoneDifferenceToUTC = TimeSpan.FromHours(2)}},
            {4, new ClockModel(){ID = 4, Name = "UTC-5", TimeZoneDifferenceToUTC = TimeSpan.FromHours(-5)}},
            {5, new ClockModel(){ID = 5, Name = "PDT", TimeZoneDifferenceToUTC = TimeSpan.FromHours(-7)} },
        };

        public async Task<bool> Add(ClockModel model)
        {
            if (model == null)
                throw new ArgumentNullException(nameof(model));
            if (model.ID != -1)
                throw new ArgumentException("The model should have an id equal to -1. Try Update to change an object", nameof(model));

            model.ID = debugList.Count == 0 ? 0 : debugList.Last().Key + 1;
            debugList.Add(model.ID, model);
            return await Task.FromResult(true);
        }


        public async Task<bool> Clear()
        {
            debugList.Clear();
            return await Task.FromResult(true);
        }


        public async Task<ClockModel> Get(int key)
        {
            if (debugList.ContainsKey(key))
                return await Task.FromResult(debugList[key]);

            return await Task.FromResult((ClockModel)null);
        }

        public async Task<IEnumerable<ClockModel>> GetAll()
        {
            return await Task.FromResult(debugList.Select(x => x.Value));
        }

        public async Task<bool> Remove(int key)
        {
            bool success = debugList.Remove(key);
            return await Task.FromResult(success);
        }

        public async Task<bool> RemoveRange(IEnumerable<int> keys)
        {
            if (keys == null)
                return await Task.FromResult(false);

            foreach (var key in keys)
                if (!debugList.ContainsKey(key))
                    return await Task.FromResult(false);

            foreach (var key in keys)
                debugList.Remove(key);

            return await Task.FromResult(true);
        }

        public async Task<bool> Update(int key, ClockModel @new)
        {
            if (!debugList.ContainsKey(key))
                return await Task.FromResult(false);

            @new.ID = key;
            debugList[key] = @new;
            return await Task.FromResult(true);
        }
    }
}

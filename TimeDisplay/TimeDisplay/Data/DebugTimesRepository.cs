using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeDisplay.Models;
using Xamarin.Forms;

namespace TimeDisplay.Data
{
    class DebugTimesRepository : IClockRepository
    {
        private readonly List<ClockModel> debugList = new List<ClockModel>()
        {
            new ClockModel(){ID = 1, Name = "UTC", TimeZoneDifferenceToUTC = TimeSpan.Zero},
            new ClockModel(){ID = 2, Name = "UTC+1", TimeZoneDifferenceToUTC = TimeSpan.FromHours(1)},
            new ClockModel(){ID = 3, Name = "UTC+2", TimeZoneDifferenceToUTC = TimeSpan.FromHours(2)},
            new ClockModel(){ID = 4, Name = "UTC-5", TimeZoneDifferenceToUTC = TimeSpan.FromHours(-5)},
            new ClockModel(){ID = 5, Name = "PDT", TimeZoneDifferenceToUTC = TimeSpan.FromHours(-7)},
        };

        public async Task<bool> Add(ClockModel model)
        {
            debugList.Add(model);
            return await Task.FromResult(true);
        }

        public async Task<bool> AddRange(IEnumerable<ClockModel> range)
        {
            if (range == null)
                return await Task.FromResult(false);

            debugList.AddRange(range);
            return await Task.FromResult(true);
        }

        public async Task<bool> Clear()
        {
            debugList.Clear();
            return await Task.FromResult(true);
        }

        public async Task<IEnumerable<ClockModel>> GetAll()
        {
            return await Task.FromResult((IEnumerable<ClockModel>)debugList);
        }

        public async Task<bool> Remove(ClockModel model)
        {
            bool success = debugList.Remove(model);
            return await Task.FromResult(success);
        }

        public async Task<bool> RemoveRange(IEnumerable<ClockModel> range)
        {
            if (range == null)
                return await Task.FromResult(false);

            debugList.RemoveAll(c => range.Contains(c));
            return await Task.FromResult(true);
        }

        public async Task<bool> Update(ClockModel old, ClockModel @new)
        {
            await Remove(old);
            await Add(@new);
            return await Task.FromResult(true);
        }
    }
}

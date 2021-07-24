using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TimeDisplay.Models;

namespace TimeDisplay.Data
{
    public interface IClockRepository : IRepository<ClockModel>
    {
        Task<bool> Clear();
    }
}

using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TimeDisplay.Models;

namespace TimeDisplay.Data.Interfaces
{
    public interface IClockRepository : IRepository<int, ClockModel>
    {
        Task<bool> Clear();
    }
}

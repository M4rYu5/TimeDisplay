using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using TimeDisplay.Models;

namespace TimeDisplay.Data
{
    public static class RepositoryFactory
    {
        private const bool useFakeValues = true;

        public static IRepository<T> GetRepository<T>()
        {
            if (typeof(T) == typeof(ClockModel))
                return useFakeValues ? (IRepository<T>) new DebugTimesRepository() : throw new NotImplementedException();

            throw new NotImplementedException();
        }
    }
}

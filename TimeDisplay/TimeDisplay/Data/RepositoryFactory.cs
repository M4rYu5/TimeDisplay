using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using TimeDisplay.Data.Interfaces;
using TimeDisplay.Models;

namespace TimeDisplay.Data
{
    public static class RepositoryFactory
    {
        private const bool useFakeValues = true;

        public static IRepository<Key, T> GetRepository<Key, T>()
        {
            if (typeof(T) == typeof(ClockModel))
                return useFakeValues ? (IRepository<Key, T>) new DebugTimesRepository() : throw new NotImplementedException();

            throw new NotImplementedException();
        }
    }
}

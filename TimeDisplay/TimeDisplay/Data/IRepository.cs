using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TimeDisplay.Models;

namespace TimeDisplay.Data
{
    public interface IRepository<T>
    {
        Task<IEnumerable<T>> GetAll();
        Task<bool> Add(T model);
        Task<bool> Remove(T model);
        Task<bool> Update(T old, T @new);
        Task<bool> AddRange(IEnumerable<T> range);
        Task<bool> RemoveRange(IEnumerable<T> range);
    }
}
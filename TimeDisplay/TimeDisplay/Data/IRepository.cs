using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TimeDisplay.Models;

namespace TimeDisplay.Data
{
    public interface IRepository<Key, T>
    {
        Task<T> Get(Key key);
        Task<IEnumerable<T>> GetAll();
        Task<bool> Add(T model);
        Task<bool> Remove(Key key);
        Task<bool> Update(Key key, T @new);
        Task<bool> RemoveRange(IEnumerable<Key> keys);
    }
}
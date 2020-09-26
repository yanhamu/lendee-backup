using Lendee.Core.Domain.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Lendee.Core.Domain.Interfaces
{
    public interface IRepository<T> where T : class
    {
        T Add(T entity);
        Task Save();
        Task<T> Remove(params object[] keys);
        ValueTask<T> Find(params object[] keys);
    }
}
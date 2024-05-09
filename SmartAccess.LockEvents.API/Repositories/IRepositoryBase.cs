using SmartAccess.LockEvents.API.Controllers;
using System.Linq.Expressions;

namespace SmartAccess.LockEvents.API.Repositories
{
    public interface IRepositoryBase<T> where T : class
    {
        Task<IEnumerable<T>> FindAllBy(LockEventsParameters lockEventsParameters, Guid? lockId, Guid? userId);
        Task<IEnumerable<T>> FindAll(LockEventsParameters lockEventsParameters);

        Task<int> Create(T lockEvent);
    }
}

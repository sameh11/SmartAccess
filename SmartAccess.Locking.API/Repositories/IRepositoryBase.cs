using System.Linq.Expressions;

namespace SmartAccess.Locking.API.Repositories
{
    public interface IRepositoryBase<T> where T : class
    {
        IEnumerable<T> FindBy(Expression<Func<T, bool>> expression);
    }
}
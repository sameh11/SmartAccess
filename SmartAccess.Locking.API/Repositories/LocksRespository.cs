using Microsoft.EntityFrameworkCore;
using SmartAccess.Locking.API.DataAccess;
using SmartAccess.Locking.API.Model;
using System.Linq.Expressions;

namespace SmartAccess.Locking.API.Repositories
{
    public class LocksRespository : IRepositoryBase<Lock>
    {
        private readonly LockingDbContext _context;

        public LocksRespository(LockingDbContext context)
        {
            _context = context;
        }
        public IEnumerable<Lock> FindBy(Expression<Func<Lock, bool>> expression)
        {
            return _context.Locks.Include(@lock=>@lock.PermissionGroups).Where(expression);
        }
    }
}

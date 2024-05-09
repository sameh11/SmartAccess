using Microsoft.EntityFrameworkCore;
using SmartAccess.Locking.API.DataAccess;
using SmartAccess.Locking.API.Model;
using System.Linq.Expressions;

namespace SmartAccess.Locking.API.Repositories
{
    public class UsersRespository : IRepositoryBase<User>
    {
        private readonly LockingDbContext _context;

        public UsersRespository(LockingDbContext context)
        {
            _context = context;
        }

        IEnumerable<User> IRepositoryBase<User>.FindBy(Expression<Func<User, bool>> expression)
        {
            return _context.Users.Include(user=>user.PermissionGroups).Where(expression);
        }
    }
}

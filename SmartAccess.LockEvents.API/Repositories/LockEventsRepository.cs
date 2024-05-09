using Microsoft.EntityFrameworkCore;
using SmartAccess.LockEvents.API.Controllers;
using SmartAccess.Services.LockEvents.API.Data;
using SmartAccess.Services.LockEvents.API.Model;
using System.Linq.Expressions;

namespace SmartAccess.LockEvents.API.Repositories
{
    public class LockEventsRepository: IRepositoryBase<LockEvent>
    {
        private readonly LockEventsDbContext _context;

        public LockEventsRepository(LockEventsDbContext context)
        {
            _context = context;
        }
        public async Task<int> Create(LockEvent lockEvent)
        {
            lockEvent.Id = Guid.NewGuid();
            _context.LockEvents.Add(lockEvent);
            return await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<LockEvent>> FindAll(LockEventsParameters lockEventsParameters)
        {
            return await _context.LockEvents
                .OrderBy(on => on.RequestDate)
                .Skip((lockEventsParameters.PageNumber - 1) * lockEventsParameters.PageSize)
                .Take(lockEventsParameters.PageSize)
                .ToListAsync();
        }

        public async Task<IEnumerable<LockEvent>> FindAllBy(LockEventsParameters lockEventsParameters, Guid? lockId, Guid? userId)
        {
            IQueryable<LockEvent> query = _context.LockEvents;

            if (lockId is not null)
            {
                query = query.Where(x => x.LockId == lockId); 
            }
            if (userId is not null)
            {
                query = query.Where(x => x.UserId == userId);
            }

            return await query.OrderBy(on => on.RequestDate)
                .Skip((lockEventsParameters.PageNumber - 1) * lockEventsParameters.PageSize)
                .Take(lockEventsParameters.PageSize).ToListAsync();
        }
    }
}

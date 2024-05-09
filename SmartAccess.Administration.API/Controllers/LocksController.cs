using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SmartAccess.Administration.API.DataAccess;
using SmartAccess.Administration.API.Model;

namespace SmartAccess.Administration.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class LocksController : ControllerBase
    {
        private readonly AdministrationDbContext _context;

        public LocksController(AdministrationDbContext context)
        {
            _context = context;
        }

        // GET: api/Locks
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Lock>>> GetLocks()
        {
            return await _context.Locks.ToListAsync();
        }

        // GET: api/Locks/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Lock>> GetLock(Guid id)
        {
            var @lock = await _context.Locks.FindAsync(id);

            if (@lock == null)
            {
                return NotFound();
            }

            return @lock;
        }

        // PUT: api/Locks/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutLock(Guid id, Lock @lock)
        {
            if (id != @lock.Id)
            {
                return BadRequest();
            }

            _context.Entry(@lock).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!LockExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Locks
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Lock>> PostLock(Lock @lock)
        {
            _context.Locks.Add(@lock);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetLock", new { id = @lock.Id }, @lock);
        }

        // DELETE: api/Locks/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteLock(Guid id)
        {
            var @lock = await _context.Locks.FindAsync(id);
            if (@lock == null)
            {
                return NotFound();
            }

            _context.Locks.Remove(@lock);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool LockExists(Guid id)
        {
            return _context.Locks.Any(e => e.Id == id);
        }
    }
}

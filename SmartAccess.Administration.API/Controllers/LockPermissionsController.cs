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

    public class LockPermissionsController : ControllerBase
    {
        private readonly AdministrationDbContext _context;

        public LockPermissionsController(AdministrationDbContext context)
        {
            _context = context;
        }

        // GET: api/LockPermissions
        [HttpGet]
        public async Task<ActionResult<IEnumerable<LockPermission>>> GetLockPermissions()
        {
            return await _context.LockPermissions.ToListAsync();
        }

        // GET: api/LockPermissions/5
        [HttpGet("{id}")]
        public async Task<ActionResult<LockPermission>> GetLockPermission(Guid id)
        {
            var lockPermission = await _context.LockPermissions.FindAsync(id);

            if (lockPermission == null)
            {
                return NotFound();
            }

            return lockPermission;
        }

        // PUT: api/LockPermissions/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutLockPermission(Guid id, LockPermission lockPermission)
        {
            if (id != lockPermission.Id)
            {
                return BadRequest();
            }

            _context.Entry(lockPermission).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!LockPermissionExists(id))
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

        // POST: api/LockPermissions
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<LockPermission>> PostLockPermission(LockPermission lockPermission)
        {
            _context.LockPermissions.Add(lockPermission);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetLockPermission", new { id = lockPermission.Id }, lockPermission);
        }

        // DELETE: api/LockPermissions/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteLockPermission(Guid id)
        {
            var lockPermission = await _context.LockPermissions.FindAsync(id);
            if (lockPermission == null)
            {
                return NotFound();
            }

            _context.LockPermissions.Remove(lockPermission);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool LockPermissionExists(Guid id)
        {
            return _context.LockPermissions.Any(e => e.Id == id);
        }
    }
}

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
    public class PermissionGroupsController : ControllerBase
    {
        private readonly AdministrationDbContext _context;

        public PermissionGroupsController(AdministrationDbContext context)
        {
            _context = context;
        }

        // GET: api/PermissionGroups
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PermissionGroup>>> GetPermissionGroups()
        {
            return await _context.PermissionGroups.ToListAsync();
        }

        // GET: api/PermissionGroups/5
        [HttpGet("{id}")]
        public async Task<ActionResult<PermissionGroup>> GetPermissionGroup(Guid id)
        {
            var permissionGroup = await _context.PermissionGroups.FindAsync(id);

            if (permissionGroup == null)
            {
                return NotFound();
            }

            return permissionGroup;
        }

        // PUT: api/PermissionGroups/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPermissionGroup(Guid id, PermissionGroup permissionGroup)
        {
            if (id != permissionGroup.Id)
            {
                return BadRequest();
            }

            _context.Entry(permissionGroup).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PermissionGroupExists(id))
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

        // POST: api/PermissionGroups
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<PermissionGroup>> PostPermissionGroup(PermissionGroup permissionGroup)
        {
            _context.PermissionGroups.Add(permissionGroup);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetPermissionGroup", new { id = permissionGroup.Id }, permissionGroup);
        }

        // DELETE: api/PermissionGroups/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePermissionGroup(Guid id)
        {
            var permissionGroup = await _context.PermissionGroups.FindAsync(id);
            if (permissionGroup == null)
            {
                return NotFound();
            }

            _context.PermissionGroups.Remove(permissionGroup);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool PermissionGroupExists(Guid id)
        {
            return _context.PermissionGroups.Any(e => e.Id == id);
        }
    }
}

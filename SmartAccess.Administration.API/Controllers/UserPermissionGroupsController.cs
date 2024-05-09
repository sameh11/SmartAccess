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
    public class UserPermissionGroupsController : ControllerBase
    {
        private readonly AdministrationDbContext _context;

        public UserPermissionGroupsController(AdministrationDbContext context)
        {
            _context = context;
        }

        // GET: api/UserPermissionGroups
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserPermissionGroup>>> GetUsersPermissionGroup()
        {
            return await _context.UsersPermissionGroup.ToListAsync();
        }

        // GET: api/UserPermissionGroups/5
        [HttpGet("{id}")]
        public async Task<ActionResult<UserPermissionGroup>> GetUserPermissionGroup(Guid id)
        {
            var userPermissionGroup = await _context.UsersPermissionGroup.FindAsync(id);

            if (userPermissionGroup == null)
            {
                return NotFound();
            }

            return userPermissionGroup;
        }

        // PUT: api/UserPermissionGroups/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUserPermissionGroup(Guid id, UserPermissionGroup userPermissionGroup)
        {
            if (id != userPermissionGroup.Id)
            {
                return BadRequest();
            }

            _context.Entry(userPermissionGroup).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserPermissionGroupExists(id))
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

        // POST: api/UserPermissionGroups
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<UserPermissionGroup>> PostUserPermissionGroup(UserPermissionGroup userPermissionGroup)
        {
            _context.UsersPermissionGroup.Add(userPermissionGroup);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetUserPermissionGroup", new { id = userPermissionGroup.Id }, userPermissionGroup);
        }

        // DELETE: api/UserPermissionGroups/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUserPermissionGroup(Guid id)
        {
            var userPermissionGroup = await _context.UsersPermissionGroup.FindAsync(id);
            if (userPermissionGroup == null)
            {
                return NotFound();
            }

            _context.UsersPermissionGroup.Remove(userPermissionGroup);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool UserPermissionGroupExists(Guid id)
        {
            return _context.UsersPermissionGroup.Any(e => e.Id == id);
        }
    }
}

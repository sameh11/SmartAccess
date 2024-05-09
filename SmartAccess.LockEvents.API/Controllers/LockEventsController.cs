using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SmartAccess.LockEvents.API.Repositories;
using SmartAccess.Services.LockEvents.API.Model;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace SmartAccess.LockEvents.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LockEventsController : ControllerBase
    {
        private readonly IRepositoryBase<LockEvent> _repository;

        public LockEventsController(IRepositoryBase<LockEvent> repository)
        {
            _repository = repository;
        }

        // GET: api/LockEvents
        [HttpGet]
        public async Task<ActionResult<IEnumerable<LockEvent>>> GetLockEvents([FromQuery] LockEventsParameters lockEventsParameters)
        {
            return Ok(await _repository.FindAll(lockEventsParameters));
        }

        // GET: api/LockEvents/User
        [HttpGet("[Action]")]
        public async Task<ActionResult<LockEvent>> GetLockEventByUserId([FromQuery] LockEventsParameters lockEventsParameters, [FromQuery] Guid userId)
        {
            var lockEvent = await _repository.FindAllBy(lockEventsParameters, null, userId);

            if (lockEvent == null)
            {
                return NotFound();
            }

            return Ok(lockEvent);
        }
        // GET: api/LockEvents/Lock
        [HttpGet("[Action]")]
        public async Task<ActionResult<LockEvent>> GetLockEventLockId([FromQuery] LockEventsParameters lockEventsParameters, [FromQuery] Guid lockId)
        {
            var lockEvent = await _repository.FindAllBy(lockEventsParameters, lockId, null);

            if (lockEvent == null)
            {
                return NotFound();
            }

            return Ok(lockEvent);
        }

        // POST: api/LockEvents
        [HttpPost]
        public async Task<ActionResult<LockEvent>> Post([FromBody] LockEvent lockEvent)
        {
            await _repository.Create(lockEvent);

            return Ok(lockEvent);
        }
    }

    public class LockEventsParameters
    {
        const int maxPageSize = 50;
        public int PageNumber { get; set; } = 1;
        private int _pageSize = 10;
        public int PageSize
        {
            get
            {
                return _pageSize;
            }
            set
            {
                _pageSize = (value > maxPageSize) ? maxPageSize : value;
            }
        }
    }
}

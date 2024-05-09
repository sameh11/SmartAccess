using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartAccess.Locking.API.Model.Response;
using SmartAccess.Locking.API.Models.Request;
using SmartAccess.Locking.API.Service;
using SmartAccess.Locking.API.Services;

namespace SmartAccess.Locking.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class LockAccessController : ControllerBase
    {
        private readonly ILogger<LockAccessController> _logger;
        private readonly ILockAccessService _accessService;
        private readonly ILockService _lockService;
        private readonly ILockEventsService _lockEventsService;
        public LockAccessController(ILogger<LockAccessController> logger,
            ILockAccessService accessService,
            ILockService lockService,
            ILockEventsService lockEventsService)
        {
            _logger = logger;
            _accessService = accessService;
            _lockService = lockService;
            _lockEventsService = lockEventsService;
        }

        [HttpPost]
        [Route("Access")]
        public async Task<IActionResult> Post([FromBody] AccessRequest accessRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                _logger.LogInformation($"Access requested UserId= {accessRequest.UserId} , LockId= {accessRequest.LockId}");


                if (_accessService.CanAccess(accessRequest.UserId, accessRequest.LockId).Result)
                {
                    await _lockEventsService.LogLockRequest(new LockEventRequest() 
                    { 
                        LockId = accessRequest.LockId,
                        UserId = accessRequest.UserId,
                        RequestDate = DateTime.Now, 
                        AccessResult = AccessResult.Granted.ToString()
                    });

                    var lockingResult = await _lockService.OpenLock(accessRequest.LockId);
                    return Ok(new LockAccessSuccess(200,lockingResult.ToString()));
                }
                else
                {
                    await _lockEventsService.LogLockRequest(new LockEventRequest()
                    {
                        LockId = accessRequest.LockId,
                        UserId = accessRequest.UserId,
                        RequestDate = DateTime.Now,
                        AccessResult = AccessResult.Denied.ToString()
                    }); ;

                    _logger.LogWarning($"Access failed UserId= {accessRequest.UserId} , LockId= {accessRequest.LockId}");
                    return Ok(new LockAccessDenied());
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(500, new LockAccessError(500, ex.Message));
            }
        }
    }
}

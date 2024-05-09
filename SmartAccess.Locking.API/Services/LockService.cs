using SmartAccess.Locking.API.Controllers;
using SmartAccess.Locking.API.Service;

namespace SmartAccess.Locking.API.Services
{
    public class LockService : ILockService
    {
        private readonly ILogger<LockService> _logger;
        public LockService(ILogger<LockService> logger)
        {
            _logger = logger;
        }
        public async Task<LockResult> OpenLock(Guid id)
        {
            //Call Service or Lock SDK and Return Result
            var lockResult = await Task.FromResult(LockResult.Open);
            if (lockResult == LockResult.Open)
            {
                _logger.LogInformation($"Lock opended, LockId= {id}");
            }
            else
            {
                _logger.LogWarning($"Access failed, LockId= {id}");
            }
            return lockResult;
        }
    }

    public enum LockResult
    {
        Open,
        LockError
    }
}

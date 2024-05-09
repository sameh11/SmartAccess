using Microsoft.Extensions.Caching.Memory;
using SmartAccess.Locking.API.Model;
using SmartAccess.Locking.API.Repositories;

namespace SmartAccess.Locking.API.Service
{
    public class LockAccessService : ILockAccessService
    {
        private readonly ILogger<LockAccessService> _logger;
        private readonly IMemoryCache _memoryCache;
        private readonly IRepositoryBase<Lock> _accessRespository;
        private readonly IRepositoryBase<User> _usersRespository;

        public LockAccessService(ILogger<LockAccessService> logger,
            IMemoryCache cache,
            IRepositoryBase<Lock> accessRespository,
            IRepositoryBase<User> usersRespository
            )
        {
            _logger = logger;
            _memoryCache = cache;
            _accessRespository = accessRespository;
            _usersRespository = usersRespository;
        }

        public async Task<bool> CanAccess(Guid userId, Guid lockId)
        {
            string cacheKey = $"{userId}:{lockId}";
            bool canAccess;
            if (_memoryCache.TryGetValue(cacheKey, out canAccess))
            {
                return canAccess;
            }
            canAccess = await DetermineUserLockAccess(userId, lockId);
            _memoryCache.Set(cacheKey, cacheKey);

            return canAccess;
        }

        private async Task<bool> DetermineUserLockAccess(Guid userId, Guid lockId)
        {
            try
            {
                var lockResponse = _accessRespository.FindBy(x=>x.Id == lockId).First();

                var userResponse = _usersRespository.FindBy(x => x.Id == userId).First();
                
                if (lockResponse.PermissionGroups.Any(x => userResponse.PermissionGroups.Any(y => y.Id == x.Id)))
                {
                    _logger.LogInformation($"Access granted UserId= {userId} , LockId= {lockId}");
                    return true;
                }
                _logger.LogWarning($"Access failed UserId= {userId} , LockId= {lockId}");
                return false;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error While getting access permission");
            }
            return false;
        }
    }
}

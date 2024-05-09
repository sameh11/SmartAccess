
namespace SmartAccess.Locking.API.Services
{
    public interface ILockService
    {
        Task<LockResult> OpenLock(Guid id);
    }
}
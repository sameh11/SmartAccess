
namespace SmartAccess.Locking.API.Service
{
    public interface ILockAccessService
    {
        Task<bool> CanAccess(Guid userId, Guid lockId);
    }
}
using SmartAccess.Locking.API.Models.Request;

namespace SmartAccess.Locking.API.Services
{
    public interface ILockEventsService
    {
        Task LogLockRequest(LockEventRequest lockevent);
    }
}
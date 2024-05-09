namespace SmartAccess.Locking.API.Controllers
{
    public class AccessRequest
    {
        public AccessRequest(Guid userId, Guid lockId)
        {
            UserId = userId;
            LockId = lockId;
        }
        public Guid LockId { get; set; }
        public Guid UserId { get; set; }

    }
}

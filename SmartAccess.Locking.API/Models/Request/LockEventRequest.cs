using System.ComponentModel.DataAnnotations;

namespace SmartAccess.Locking.API.Models.Request
{
    public class LockEventRequest
    {
        public DateTime RequestDate { get; set; }

        public Guid UserId { get; set; }

        public Guid LockId { get; set; }

        public string AccessResult { get; set; }
    }
    public enum AccessResult
    {
        Denied,
        Granted 
    }
}

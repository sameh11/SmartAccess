using System.ComponentModel.DataAnnotations;

namespace SmartAccess.Services.LockEvents.API.Model
{
    public class LockEvent
    {
        public Guid Id { get; set; }

        [Required(ErrorMessage = "RequestDate is required")]
        public DateTime RequestDate { get; set; }

        [Required(ErrorMessage = "UserId is required")]
        public Guid UserId { get; set; }

        [Required(ErrorMessage = "LockId is required")]
        public Guid LockId { get; set; }

        [Required(ErrorMessage = "Command is required")]
        [StringLength(10, ErrorMessage = "Command can't be longer than 10 characters")]
        public string AccessResult { get; set; }        
    }
}

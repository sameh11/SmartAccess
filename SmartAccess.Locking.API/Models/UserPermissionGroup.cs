using System.ComponentModel.DataAnnotations;

namespace SmartAccess.Locking.API.Model
{
    public class UserPermissionGroup
    {
        [Key]
        public Guid Id { get; set; }

        [Required(ErrorMessage = "PermissionGroupId is required")]
        public Guid PermissionGroupId { get; set; }

        [Required(ErrorMessage = "UserId is required")]
        public Guid UserId { get; set; }

        //public PermissionGroup PermissionGroup { get; set; }

        //public User User { get; set; }
    }
}

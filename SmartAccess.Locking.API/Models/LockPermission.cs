using System.ComponentModel.DataAnnotations;

namespace SmartAccess.Locking.API.Model
{
    public class LockPermission
    {
        [Key]
        public Guid Id { get; set; }

        [Required(ErrorMessage = "Lock is required")]
        public Guid LockId { get; set; }

        [Required(ErrorMessage = "PermissionGroup is required")]
        public Guid PermissionGroupId { get; set; }

        public bool CanAccess { get; set; } = true;

        //public List<Lock> Locks { get; set; }

        //public List<PermissionGroup> PermissionGroups { get; set; }
    }
}

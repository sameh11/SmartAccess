using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SmartAccess.Administration.API.Model
{
    public class LockPermission
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
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

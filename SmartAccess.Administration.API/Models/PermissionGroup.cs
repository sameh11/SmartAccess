using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SmartAccess.Administration.API.Model
{
    public class PermissionGroup
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        [Required(ErrorMessage = "Name is required")]
        [StringLength(20, ErrorMessage = "Name can't be longer than 60 characters")]
        public string Name { get; set; }


        public List<User> Users { get; set; }
        public List<UserPermissionGroup> UsersPermissionGroup { get; set; }
        public List<Lock> Locks { get; set; }
        public List<LockPermission> LockPermissions { get; set; }
    }
}

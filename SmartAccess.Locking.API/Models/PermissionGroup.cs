using System.ComponentModel.DataAnnotations;

namespace SmartAccess.Locking.API.Model
{
    public class PermissionGroup
    {
        [Key]
        public Guid Id { get; set; }

        [Required(ErrorMessage = "Name is required")]
        [StringLength(20, ErrorMessage = "Name can't be longer than 60 characters")]
        public string Name { get; set; }

        public List<User> Users { get; set; }
        public List<Lock> Locks { get; set; }
    }
}

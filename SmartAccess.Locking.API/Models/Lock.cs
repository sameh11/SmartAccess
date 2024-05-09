using System.ComponentModel.DataAnnotations;

namespace SmartAccess.Locking.API.Model
{
    public class Lock
    {
        [Key]
        public Guid Id { get; set; }

        [Required(ErrorMessage = "Name is required")]
        [StringLength(20, ErrorMessage = "Name can't be longer than 20 characters")]
        public string Name { get; set; }

        public List<PermissionGroup> PermissionGroups { get; set; }
    }
}

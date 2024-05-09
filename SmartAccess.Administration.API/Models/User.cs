using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SmartAccess.Administration.API.Model
{
    public class User
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        [Required(ErrorMessage = "Name is required")]
        [StringLength(20, ErrorMessage = "Name can't be longer than 20 characters")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Family is required")]
        [StringLength(20, ErrorMessage = "LastName can't be longer than 20 characters")]
        public string LastName { get; set; }

        [NotMapped]
        public string Role { get; set; }

        public List<PermissionGroup> PermissionGroups { get; set; } = new List<PermissionGroup>();
        public List<UserPermissionGroup> UsersPermissionGroup { get; set; } = new List<UserPermissionGroup>();

    }
}

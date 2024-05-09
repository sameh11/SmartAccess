using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SmartAccess.Administration.API.Model
{
    public class UserPermissionGroup
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        [Required(ErrorMessage = "PermissionGroupId is required")]
        public Guid PermissionGroupId { get; set; }

        [Required(ErrorMessage = "UserId is required")]
        public Guid UserId { get; set; }

        public PermissionGroup PermissionGroup { get; set; }

        public User User { get; set; }
    }
}

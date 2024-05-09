using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SmartAccess.Administration.API.Model
{
    public class Lock
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required(ErrorMessage = "Name is required")]
        [StringLength(20, ErrorMessage = "Name can't be longer than 20 characters")]
        public string Name { get; set; }

        public bool IsOpened { get; set; }

        public virtual List<PermissionGroup> PermissionGroups { get; set; }
    }
}

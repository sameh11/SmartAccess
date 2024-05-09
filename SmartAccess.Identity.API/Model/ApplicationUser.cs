using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace SmartAccess.Services.Identity.Model
{
    public class ApplicationUser : IdentityUser
    {
        [Required]
        public string Expiration { get; set; }      
        [Required]
        public string Name { get; set; }
        [Required]
        public string LastName { get; set; }
        
    }
}

using Microsoft.AspNetCore.Identity;

namespace EventiAPI.Models
{
    public class ApplicationUser : IdentityUser
    {
        public ICollection<Biglietto> Biglietti { get; set; }
    }
}

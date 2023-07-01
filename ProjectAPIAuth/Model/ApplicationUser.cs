using Microsoft.AspNetCore.Identity;

namespace ProjectAPIAuth.Model
{
    public class ApplicationUser : IdentityUser
    {
        public string Name { get; set; }

    }
}

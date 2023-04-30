using Microsoft.AspNetCore.Identity;

namespace ProjectAPI.Model
{
    public class ApplicationUser : IdentityUser
    {
        public string Name { get; set; }

    }
}

using Microsoft.AspNetCore.Identity;

namespace Core.Entities
{
    public class AppUser : IdentityUser
    {
        public DateTime CreatedAt { get; set; }
        public UserType UserType { get; set; }
    }
}

using Microsoft.AspNetCore.Identity;

namespace Identity.Models.ViewModels
{
    public class User : IdentityRole
    {
        public string Name { get; set; }

        public string Role { get; set; }
    }
}

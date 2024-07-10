using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace Identity.Models
{
    public class ApplicationUser :IdentityUser
    {
        public string? Name { get; set; }
    }
}

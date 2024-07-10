using Identity.Data;
using Identity.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace AppointmentScheduler.Data
{
    public class DbInitializer : IDbInitialize
    {
        private readonly ApplicationDbContext db;
        public UserManager<ApplicationUser> userManager { get; }
        public RoleManager<IdentityRole> roleManager { get; }

        public DbInitializer(ApplicationDbContext _db,UserManager<ApplicationUser> _userManager, RoleManager<IdentityRole> _roleManager) 
        {
            db = _db;
            roleManager = _roleManager;
            userManager = _userManager;
        }


        public void Initialize()
        {
            try
            {
                if(db.Database.GetPendingMigrations().Count() > 0)
                {
                    db.Database.Migrate();
                }
            }catch (Exception ex)
            {

            }
            if (db.Roles.Any(x => x.Name == Identity.Utilities.Helper.Admin)) return;

                roleManager.CreateAsync(new IdentityRole(Identity.Utilities.Helper.Admin)).GetAwaiter().GetResult();

            userManager.CreateAsync(new ApplicationUser
            {
                UserName = "SuperAdmin@gmail.com",
                Email = "superadmin@gmail.com",
                EmailConfirmed = true,
                Name = "Asif Ali"
            }, "Asd123@").GetAwaiter().GetResult();
            ApplicationUser user = db.Users.FirstOrDefault(x => x.Email == "superadmin@gmail.com");
            userManager.AddToRoleAsync(user, Identity.Utilities.Helper.Admin).GetAwaiter().GetResult();
        }
    }
}

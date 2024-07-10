using Identity.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
namespace Identity.Controllers
{
    public class RoleController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public RoleController(RoleManager<IdentityRole> userRole,UserManager<ApplicationUser> userManager )
        {
           _roleManager= userRole;
           _userManager= userManager;
        }
        [HttpGet]
        public IActionResult Index()
        {
              var  roles =   _roleManager.Roles.ToList();   

            return View(roles);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(IdentityRole input)
        {
            if (!string.IsNullOrEmpty(input.Name))
            {
                var role = new IdentityRole { Name = input.Name, NormalizedName = input.Name.ToUpper() };
                var result = await _roleManager.CreateAsync(role);
                if (result.Succeeded) 
                {
                    return RedirectToAction("Index");
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }
            return View();
        }

        [HttpGet]
        public async  Task<IActionResult> Update(string Id)
        {
            if (Id == null)
            {
                return NotFound();
            }
            var item = await _roleManager.FindByIdAsync(Id);
            if (item == null)
            {
                return NotFound();
            }
            return View(item);
            
        }

        [HttpPost]
        public async Task<IActionResult> Update(IdentityRole input)
        {

         //   if(User.IsInRole(Identity.Utilities))

            if (!string.IsNullOrEmpty(input.Name)  )
            {
                IdentityRole role = await _roleManager.FindByIdAsync(input.Id);
                if (input.Name == null)
                {
                    return BadRequest();
                }
                if (input.Name != role.Name)
                {

                    role.Name = input.Name;
                }
                var result = await _roleManager.UpdateAsync(role);
                if (result.Succeeded)
                {
                   return RedirectToAction("Index");
                }
                 foreach (var error in result.Errors)
                 {
                     ModelState.AddModelError("", error.Description);
                 }
            }
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Delete(string Id)
        {

           if (Id == null)
           {
               return NotFound();
           }
           var item = await _roleManager.FindByIdAsync(Id);
            if (item == null)
            {
               return NotFound();
             }
            
            return View(item);
        }

        [HttpPost]
        public async Task<IActionResult> DeletePost(string id)
        {

            if (User.IsInRole(Identity.Utilities.Helper.Admin))
            {
                var role = await _roleManager.FindByIdAsync(id);
              //  var user = await _userManager.GetUsersInRoleAsync(role.Name);
                if (role != null /*&& user == null*/)
                {
                    var result = await _roleManager.DeleteAsync(role);
                    if (!result.Succeeded)
                    {
                        throw new Exception("Failed to delete role.");
                    }
                    else
                    {
                        foreach (var error in result.Errors)
                        {
                            ModelState.AddModelError("", error.Description);
                        }
                    }

                }
                return View("Index");
            }
            return View("Index");
        }
    }
}

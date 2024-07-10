using Identity.Models.ViewModels;
using Identity.Data;
using Identity.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
namespace identity.Controllers
{
    public class AccountController : Controller
    {
        private readonly ApplicationDbContext _db;
        SignInManager<ApplicationUser> _signInManager;
        UserManager<ApplicationUser> _userManager;
        RoleManager<IdentityRole>  _roleManager;

        public AccountController(ApplicationDbContext db, 
        SignInManager<ApplicationUser> signInManager,
        UserManager<ApplicationUser> userManager,
        RoleManager<IdentityRole> roleManager)
        {
            this._db = db;
            _signInManager= signInManager;
            _userManager= userManager;
            _roleManager= roleManager;
        }
        public IActionResult Index()
        {
            var aa = _db.Users.ToList();
            return View(aa);
        }
        public  IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> login(LoginModel loginModel)
        {
            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(loginModel.Email, loginModel.Password, loginModel.RememberMe,false);
                if(result.Succeeded)
                {
                    var user = await _userManager.FindByNameAsync(loginModel.Email);
                    if (_signInManager.IsSignedIn(User))
                    {
                        return RedirectToAction("Index", "Item");
                    }
                    return RedirectToAction("Index", "Item");
                }
            }
            ModelState.AddModelError("", "Invalid Login attempt");
            return View(loginModel);
        }
        public IActionResult Register()
        {
            var model = new RegisterModel();
            var roles =  _db.Roles.Select(e => new SelectListItem
            {
                Value = e.Name,
                Text = e.Name

            }).ToList();

             model.RolesList = roles;
            
            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterModel registerModel)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser
                {
                    UserName = registerModel.EmailAddress,
                    Email = registerModel.EmailAddress,
                    NormalizedUserName=registerModel.EmailAddress.ToUpper(),
                    NormalizedEmail=registerModel.EmailAddress.ToUpper(),    
                    Name = registerModel.RoleName,
                };

                var result = await _userManager.CreateAsync(user,registerModel.Password);
                if (result.Succeeded)
                {
                    var role = await _roleManager.FindByNameAsync("User");
                    await _userManager.AddToRoleAsync(user, registerModel.RoleName);
                    await _signInManager.SignInAsync(user,isPersistent: false);
                    return RedirectToAction("Index","Item");
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }   
            }
            return View();
        }


        //[HttpGet]
        //public async Task<IActionResult> Update(string Id)
        //{
        //    if (Id == null)
        //    {
        //        return NotFound();
        //    }
        //    var item = await _userManager.FindByIdAsync(Id);
        //    if (item == null)
        //    {
        //        return NotFound();
        //    }
        //    return View(item);

        //}

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Update(IdentityUser input)
        //{

        //    if (!string.IsNullOrEmpty(input.UserName) && !string.IsNullOrEmpty(input.Email) )
        //    {
        //        var role = await _userManager.FindByIdAsync(input.Id);
        //        if (input.UserName == null && input.Email == null)
        //        {
        //            return BadRequest();
        //        }
        //        if (input.UserName != role.UserName && input.Email != role.Email)
        //        {
        //            role.Email = input.Email;
        //            role.UserName = input.UserName;
        //        }
        //        var result = await _userManager.UpdateAsync(role);
        //        if (result.Succeeded)
        //        {
        //            return RedirectToAction("Index");
        //        }
        //        foreach (var error in result.Errors)
        //        {
        //            ModelState.AddModelError("", error.Description);
        //        }
        //    }
        //    return View();
        //}


        [HttpGet]
        public async Task<IActionResult> Delete(string Id)
        {

            if (Id == null)
            {
                return NotFound();
            }
            var item = await _userManager.FindByIdAsync(Id);
            if (item == null)
            {
                return NotFound();
            }

            return View(item);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeletePost(string id)
        {

            if (User.IsInRole(Identity.Utilities.Helper.Admin))
            {
                var role = await _userManager.FindByIdAsync(id);
                
                if (role != null)
                {
                    var result = await _userManager.DeleteAsync(role);
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





        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Login","Account");
        }

    }
}

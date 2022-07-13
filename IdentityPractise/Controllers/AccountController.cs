using IdentityPractise.Helpers;
using IdentityPractise.Models;
using IdentityPractise.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using SignInResult = Microsoft.AspNetCore.Identity.SignInResult;

namespace IdentityPractise.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<AppUser> _userManager;


        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly SignInManager<AppUser> _signInManager;
        public AccountController(UserManager<AppUser> userManager,
            RoleManager<IdentityRole> roleManager, SignInManager<AppUser> signInManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _signInManager = signInManager;
        }
        public IActionResult Register()
        {
            if (User.Identity.IsAuthenticated) return RedirectToAction("index", "home");
            
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterVM register)
        {
            if (User.Identity.IsAuthenticated) return RedirectToAction("index", "home");
            if (!ModelState.IsValid) return View();
            AppUser user = new AppUser
            {
                Fullname = register.Fullname,
                UserName = register.Username,
                Email = register.Email,
            };

            IdentityResult result = await _userManager.CreateAsync(user, register.Password);
            if (!result.Succeeded)
            {
                foreach (var item in result.Errors)
                {
                    ModelState.AddModelError("", item.Description);
                }
                return View(register);
            }
            await _userManager.AddToRoleAsync(user, UserRoles.Member.ToString());
            
            return RedirectToAction("login", "account");
        }

        public IActionResult Login()
        {
            if (User.Identity.IsAuthenticated) return RedirectToAction("index", "home");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginVM login)
        {
            if (User.Identity.IsAuthenticated) return RedirectToAction("index", "home");
            if (!ModelState.IsValid) return NotFound();
            AppUser user = await _userManager.FindByEmailAsync(login.Email);
            
            if(user == null)
            {
                ModelState.AddModelError("", "Incorrect email or password");
                return View(login);
            }
            SignInResult result =await _signInManager.PasswordSignInAsync(user, login.Password, login.RememberMe, true);
            if (result.IsLockedOut)
            {
                ModelState.AddModelError("", "Profile is blocked");
                return View(login);
            }
            if (!result.Succeeded)
            {
                ModelState.AddModelError("", "Incorrect email or password");
                return View(login);
            }

            await _signInManager.SignInAsync(user, login.RememberMe);
            return RedirectToAction("index", "home");
        }

        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("index", "home");
        }


        /*public async Task CreateRole()
        {
            foreach (var item in Enum.GetValues(typeof(UserRoles)))
            {
                if (!await _roleManager.RoleExistsAsync(item.ToString()))
                {
                await _roleManager.CreateAsync(new IdentityRole { Name = item.ToString() });

                }
            }
        }*/
    }
}

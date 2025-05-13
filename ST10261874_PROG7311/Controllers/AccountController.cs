using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ST10261874_PROG7311.Models;
using ST10261874_PROG7311.ViewModels;
using System.Threading.Tasks;

namespace ST10261874_PROG7311.Controllers
{
    public class AccountController : Controller
    {
        //dependency injection of UserManager and SignInManager for handling identity operations
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        //constructor to initialise UserManager and SignInManager
        public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        //displays the login view
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        //handles login logic
        [HttpPost]
        [ValidateAntiForgeryToken] 
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            //check if the form model is valid
            if (ModelState.IsValid)
            {
                //attempt to sign the user in with the provided credentials
                var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, false);
                
                if (result.Succeeded)
                {
                    //fetching by email
                    var user = await _userManager.FindByEmailAsync(model.Email);
                    if (user != null)
                    {
                        //hardcoded employee login
                        if (model.Email == "employee@gmail.com")
                        {
                            return RedirectToAction("Index", "Employee");
                        }
                    
                        else if (await _userManager.IsInRoleAsync(user, "Farmer"))
                        {
                            return RedirectToAction("Index", "Farmer");
                        }
                    }

                    //default redirect if no specific role match
                    return RedirectToAction("Index", "Home");
                }

                //login failed: show error message
                ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                ViewData["InvalidLoginAttempt"] = "Invalid email or password.";
            }

            //if model state is invalid or login fails, return the same view with error
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
    }
}

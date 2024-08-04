using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using RunBuddies.App.Models;
using RunBuddies.DataModel;

namespace RunBuddies.App.Controllers
{
    public class ProfileController : Controller
    {
        private readonly UserManager<User> userManager;

        private readonly SignInManager<User> signInManager;

        public ProfileController(UserManager<User> userManager, SignInManager<User> signInManager)
        {
            this.userManager = userManager;

            this.signInManager = signInManager;
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel Model)
        {
            if (ModelState.IsValid)
            {
                User New = new();

                New.UserName = Model.UserName;
                New.Email = Model.Email;
                New.FirstName = Model.FirstName;
                New.LastName = Model.LastName;
                New.Birthday = Model.Birthday;
                New.Gender = Model.Gender;
                New.PhoneNumber = Model.PhoneNumber;

                await userManager.CreateAsync(New, Model.Password);

                return RedirectToAction("SignIn", "Profile");
            }
            else
            {
                return View(Model);
            }
        }

        public IActionResult SignIn(string? returnURL)
        {
            SignInViewModel vm = new();

            if (!string.IsNullOrEmpty(returnURL))
                vm.ReturnUrl = returnURL;

            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SignIn(SignInViewModel Model, string? returnURL)
        {
            if (ModelState.IsValid)
            {
                User? user = (User?)await userManager.FindByNameAsync(Model.UserName);

                if (user != null)
                {
                    var result = await signInManager.PasswordSignInAsync(user, Model.Password, false, false);

                    if (result.Succeeded)
                    {
                        if (!string.IsNullOrEmpty(returnURL))
                            return LocalRedirect(returnURL);

                        return LocalRedirect("/Home/Index");
                    }
                    else
                    {
                        ModelState.AddModelError("Error", "Invalid Credentials");

                        return View(Model);
                    }
                }
                else
                {
                    ModelState.AddModelError("Error", "Invalid Credentials");

                    return View(Model);
                }
            }
            else
            {
                return View(Model);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public new async Task<IActionResult> SignOut()
        {
            await signInManager.SignOutAsync();

            return RedirectToAction("SignIn", "Profile");
        }
    }
}

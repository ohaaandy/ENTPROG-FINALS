using Microsoft.AspNetCore.Authorization;
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
        [Authorize]
        public async Task<IActionResult> Index()
        {
            var user = await userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound();
            }

            var viewModel = new ProfileViewModel
            {
                UserName = user.UserName,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Birthday = user.Birthday,
                Gender = user.Gender,
                RunnerLevel = user.RunnerLevel,
                Location = user.Location,
                PreferredDay = user.Schedule?.DayOfWeek,
                Distance = user.Distance
            };

            if (!user.IsProfileComplete)
            {
                ViewBag.Message = "Please complete your profile to continue.";
            }

            return View(viewModel);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> UpdateProfile(ProfileViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View("Index", model);
            }

            var user = await userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound();
            }

            user.FirstName = model.FirstName;
            user.LastName = model.LastName;
            user.Birthday = model.Birthday;
            user.Gender = model.Gender;
            user.RunnerLevel = model.RunnerLevel;
            user.Location = model.Location;
            if (model.PreferredDay.HasValue)
            {
                var today = DateOnly.FromDateTime(DateTime.Today);
                int daysUntilPreferred = ((int)model.PreferredDay.Value - (int)today.DayOfWeek + 7) % 7;
                user.Schedule = today.AddDays(daysUntilPreferred);
            }
            else
            {
                user.Schedule = null;
            }
            user.Distance = model.Distance;
            user.IsProfileComplete = true;  // Set this to true after updating the profile

            var result = await userManager.UpdateAsync(user);
            if (result.Succeeded)
            {
                return RedirectToAction("Index", "Home");
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }

            return View("Index", model);
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
                User New = new()
                {
                    UserName = Model.UserName,
                    Email = Model.Email,
                    FirstName = Model.FirstName,
                    LastName = Model.LastName,
                    Birthday = Model.Birthday,
                    Gender = Model.Gender,
                    PhoneNumber = Model.PhoneNumber,
                    IsProfileComplete = false  // Set this to false for new users
                };

                var result = await userManager.CreateAsync(New, Model.Password);

                if (result.Succeeded)
                {
                    await signInManager.SignInAsync(New, isPersistent: false);
                    return RedirectToAction("Index", "Profile");
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            return View(Model);
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
                User? user = await userManager.FindByNameAsync(Model.UserName);

                if (user != null)
                {
                    var result = await signInManager.PasswordSignInAsync(user, Model.Password, false, false);

                    if (result.Succeeded)
                    {
                        if (!user.IsProfileComplete)
                        {
                            return RedirectToAction("Index", "Profile");
                        }

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

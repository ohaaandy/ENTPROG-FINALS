﻿using Microsoft.AspNetCore.Authorization;
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
                // Create a DateOnly for the next occurrence of the selected day
                var today = DateOnly.FromDateTime(DateTime.Today);
                int daysUntilPreferred = ((int)model.PreferredDay.Value - (int)today.DayOfWeek + 7) % 7;
                user.Schedule = today.AddDays(daysUntilPreferred);
            }
            else
            {
                user.Schedule = null;
            }
            user.Distance = model.Distance;

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

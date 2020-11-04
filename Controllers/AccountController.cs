using iText.Layout.Element;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TMOffersClients;
using TMOffersClients.Models;

namespace EmployeeManagement
{
    public class AccountController : Controller
    {
        private readonly UserManager<TMUser> mUserManager;
        private readonly SignInManager<TMUser> mSignInManager;
        private readonly RoleManager<IdentityRole> mRoleManager;

        public AccountController(UserManager<TMUser> userManager,
                                    SignInManager<TMUser> signInManager,
                                    RoleManager<IdentityRole> roleManager)
        {
            mUserManager = userManager;
            mSignInManager = signInManager;
            mRoleManager = roleManager;
        }

        /// <summary>
        /// Logout the current user
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> Logout()
        {
            await mSignInManager.SignOutAsync();
            return RedirectToAction("index", "home");
        }

        /// <summary>
        /// Checks to see whther the input email is in use
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        [HttpPost]
        [HttpGet]
        [AllowAnonymous]
        public async Task<JsonResult> IsEmailInUse(string email)
        {
            var user = await mUserManager.FindByNameAsync(email);

            if (user == null)
                return Json(true);
            else
                return Json($"Email {email} вече се използва от друг потребител");
        }

        /// <summary>
        /// Get the Login view
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        public IActionResult Login()
        {
            //var superUser = new TMUser { UserName = "TMV_Stanislav", Email = "s.kolev@trademeister.bg" };

            //var userCreation = await mUserManager.CreateAsync(superUser, "staniSlavle#1990");

            //IdentityResult roleCreation;

            //if (userCreation.Succeeded)
            //{
            //    roleCreation = await mRoleManager.CreateAsync(new IdentityRole { Name = "SuperAdmin" });
            //    roleCreation = await mRoleManager.CreateAsync(new IdentityRole { Name = "Admin" });
            //    roleCreation = await mRoleManager.CreateAsync(new IdentityRole { Name = "User" });
            //}
            //else
            //    return View();

            //IdentityResult assignRole;

            //if (roleCreation.Succeeded)
            //{
            //    assignRole = await mUserManager.AddToRoleAsync(superUser, "SuperAdmin");
            //}
            //else
            //    return View();


            return View();
        }

        /// <summary>
        /// Login user
        /// </summary>
        /// <param name="model"></param>
        /// <param name="returnUrl"></param>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login(LoginViewModel model, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                var result = await mSignInManager.PasswordSignInAsync(model.Name, model.Password, false, false);

                if (result.Succeeded)
                {
                    if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                        return Redirect(returnUrl);
                    else
                        return RedirectToAction("index", "home");
                }

                ModelState.AddModelError(string.Empty, "Invalid Login Attempt");
            }

            return RedirectToAction("index", "home");
        }

        /// <summary>
        /// Get the register user view
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Authorize(Roles = "SuperAdmin, Admin")]
        public IActionResult Register()
        {
            return View();
        }

        /// <summary>
        /// Registering user
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize(Roles = "SuperAdmin, Admin")]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            var userExists = await mUserManager.FindByNameAsync(model.Name);
            if (userExists != null)
            {
                ViewBag.ErrorMessage = "Потребител с такова име вече съществува!";
                ViewBag.SourceLink = "/account/register";
                return View("ErrorMessage");
            }

            var user = new TMUser
            {
                UserName = model.Name,
                Email = model.Email
            };

            if (ModelState.IsValid)
            {
                var userCreated = await mUserManager.CreateAsync(user, model.Password);
                IdentityResult assignRole;
                if (userCreated.Succeeded)
                {
                    assignRole = await mUserManager.AddToRoleAsync(user, ((Role)(int.Parse(model.Role))).ToString());
                    if (assignRole.Succeeded)
                        return RedirectToAction("index", "home");
                    else
                    {
                        ViewBag.ErrorMessage = "Неуспешно назначаване на ниво на досъп на потребител!";
                        ViewBag.SourceLink = "/account/register";
                        return View("ErrorMessage");
                    }
                }
                else
                {
                    ViewBag.ErrorMessage = "Неуспешно създаване на потребител!";
                    ViewBag.SourceLink = "/account/register";
                    return View("ErrorMessage");
                }

            }

            return View("Register");
        }

        /// <summary>
        /// Get the EditUsers view
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> EditUsers(ListUsersViewModel model)
        {
            model.Users = new List<User>();

            var users = mUserManager.Users;

            foreach (var user in users)
            {
                var userRoles = await mUserManager.GetRolesAsync(user);
                if (!await mUserManager.IsInRoleAsync(user, "SuperAdmin"))
                    model.Users.Add(new User { Id = user.Id, Name = user.UserName, Email = user.Email, Role = userRoles[0] });
            }

            return View(model);
        }

        /// <summary>
        /// Get the EditUser view
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> EditUser(string id)
        {
            var user = await mUserManager.FindByIdAsync(id);
            var roles = await mUserManager.GetRolesAsync(user);

            var model = new UpdateUserViewModel
            {
                Name = user.UserName,
                Email = user.Email,
                Role = roles[0]
            };

            ViewBag.Title = "Корекция на данните за потребител ";
            ViewBag.Type = false;
            ViewBag.UserId = user.Id;

            return View(model);
        }

        /// <summary>
        /// Updating user
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> EditUser(UpdateUserViewModel model, string id)
        {
            if (ModelState.IsValid)
            {
                var currUser = await mUserManager.FindByIdAsync(id);

                currUser.UserName = model.Name;
                currUser.Email = model.Email;
                var changePassword = await mUserManager.ChangePasswordAsync(currUser, model.CurrentPassowrd, model.Password);

                if (!changePassword.Succeeded)
                {
                    ViewBag.ErrorMessage = $"Неуспешна смяна на парола на потребител {currUser.UserName}!";
                    ViewBag.SourceLink = $"/account/edituser/{id}";
                    return View("ErrorMessage");
                }

                var updateUser = await mUserManager.UpdateAsync(currUser);
                if (!updateUser.Succeeded)
                {
                    ViewBag.ErrorMessage = $"Неуспешна промяна на данни за потребител {currUser.UserName}!";
                    ViewBag.SourceLink = $"/account/edituser/{id}";
                    return View("ErrorMessage");
                }

                return RedirectToAction("editusers");
            }

            ViewBag.ErrorMessage = $"Грешно въвеждане на данни!";
            ViewBag.SourceLink = $"/account/edituser/{id}";
            return View("ErrorMessage");
        }

        /// <summary>
        /// Get the DeleteUser view
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> DeleteUser(string id)
        {
            var user = await mUserManager.FindByIdAsync(id);
            var roles = await mUserManager.GetRolesAsync(user);

            var model = new UpdateUserViewModel
            {
                Name = user.UserName,
                Email = user.Email,
                Role = roles[0]
            };

            ViewBag.Title = "Сигурни ли сте, че искате да изтриете потребител ";
            ViewBag.Type = true;
            ViewBag.UserId = user.Id;

            return View(model);
        }

        /// <summary>
        /// Deleting user
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> DeleteUser(UpdateUserViewModel model, string id)
        {
            var currUser = await mUserManager.FindByIdAsync(id);

            var deleteUser = await mUserManager.DeleteAsync(currUser);

            if (!deleteUser.Succeeded)
            {
                ViewBag.ErrorMessage = $"Неуспешно изтриване на потребител {currUser.UserName}!";
                ViewBag.SourceLink = $"/account/edituser/{id}";
                return View("ErrorMessage");
            }

            return RedirectToAction("editusers");
        }
    }
}

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using TMOffersClients.Models;

namespace TMOffersClients.Controllers
{
    public class HomeController : Controller
    {

        private readonly SignInManager<TMUser> mSignInManager;

        public HomeController(SignInManager<TMUser> signInManager)
        {
            mSignInManager = signInManager;
        }

        public IActionResult Index()
        {
            if (mSignInManager.IsSignedIn(User))
                return View();           

            return RedirectToAction("login", "account");
        }
    }
}
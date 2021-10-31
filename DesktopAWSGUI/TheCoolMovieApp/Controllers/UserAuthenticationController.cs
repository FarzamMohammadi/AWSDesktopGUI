using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TheCoolMovieApp.Models;

namespace TheCoolMovieApp.Controllers
{
    public class UserAuthenticationController : Controller
    {
        //Returns Login view
        public IActionResult Login()
        {
            return View();
        }

        //Returns Sign up View
        public IActionResult SignUp()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Subscribe(UserModel model)
        {
            if (ModelState.IsValid)
            {
                //TODO: UserModel(model.Email);
            }

            return View("Index", model);
        }
    }
}

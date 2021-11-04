using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TheCoolMovieApp.Controllers
{
    public class MovieController : Controller
    {
        public IActionResult ViewAllMovies()
        {
            return View();
        }

        public IActionResult AddMovie()
        {
            return View();
        }

        [HttpPost]
        public ActionResult RedirectToAddNewMovie()
        {
            return View("AddMovie");
        }
    }
}

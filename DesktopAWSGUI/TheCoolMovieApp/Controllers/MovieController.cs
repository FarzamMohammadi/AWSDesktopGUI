using Amazon.S3.Model;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TheCoolMovieApp.Models;

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

        public ActionResult RedirectToAddNewMovie()
        {
            return View("AddMovie");
        }
 
        [HttpPost]
        public ActionResult AddMovie(MovieModel newMovie)
        {
            ClientModel client = new ClientModel();
            return View("AddMovie", newMovie);
        }
    }
}

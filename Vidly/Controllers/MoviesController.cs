using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Vidly.DataAccessLayer;
using Vidly.Models;
using Vidly.ViewModels;

namespace Vidly.Controllers
{
    public class MoviesController : Controller
    {
        // GET: Movies/Random
        public ActionResult Random()
        {
            var movie = new Movie() { Name = "Abyss" };
            return View(movie);
        }

        public ActionResult Index()
        {
            var movies = GetMovies();

            return View(movies);
        }

        public ActionResult New()
        {
            VidlyDAL vidlyDAL = new VidlyDAL();
            var genres = vidlyDAL.Genres.ToList();

            var viewModel = new MovieFormViewModel
            {
                Movie = new Movie(),
                Genres = genres
            };

            return View("MovieForm", viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Save(Movie movie)
        {
            VidlyDAL vidlyDAL = new VidlyDAL();
            if (!ModelState.IsValid)
            {
                var viewModel = new MovieFormViewModel
                {
                    Movie = movie,
                    Genres = vidlyDAL.Genres.ToList()
                };

                return View ("MovieForm", viewModel);
            }

            if (movie.ID == 0)
            {
                vidlyDAL.Movies.Add(movie);
            }
            else
            {
                var movieInDb = vidlyDAL.Movies.Single(m => m.ID == movie.ID);
                movieInDb.Name = movie.Name;
                movieInDb.DateAdded = movie.DateAdded;
                movieInDb.ReleaseDate = movie.ReleaseDate;
                movieInDb.NumberInStock = movie.NumberInStock;
                movieInDb.GenreID = movie.GenreID;
            }
            vidlyDAL.SaveChanges();

            return RedirectToAction("Index", "Movies");
        }

        public ActionResult Details(int id)
        {
            var movie = GetMovies().SingleOrDefault(m => m.ID == id);

            if (movie == null)
            {
                return HttpNotFound();
            }

            return View(movie);
        }

        public ActionResult Edit(int id)
        {
            var movie = GetMovies().SingleOrDefault(m => m.ID == id);

            if (movie == null)
            {
                return HttpNotFound();
            }
            VidlyDAL vidlyDAL = new VidlyDAL();
            var viewModel = new MovieFormViewModel
            {
                Movie = movie,
                Genres = vidlyDAL.Genres.ToList()
            };
            return View("MovieForm", viewModel);
        }

        private IEnumerable<Movie> GetMovies()
        {
            //return new List<Movie>
            //{
            //    new Movie { Name = "Alpha", ID = 1 },
            //    new Movie { Name = "Beta", ID = 2 }
            //};

            VidlyDAL vidlyDAL = new VidlyDAL();
            return vidlyDAL.Movies.ToList<Movie>();
        }
    }
}
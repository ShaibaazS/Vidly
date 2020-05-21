using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Vidly.DataAccessLayer;
using Vidly.Models;

namespace Vidly.Controllers.API
{
    public class MoviesController : ApiController
    {
        //GET /api/movies
        public IEnumerable<Movie> GetMovies()
        {
            VidlyDAL vidlyDAL = new VidlyDAL();

            return vidlyDAL.Movies.ToList();
        }

        //GET /api/movies/1
        public Movie GetMovie(int id)
        {
            VidlyDAL vidlyDAL = new VidlyDAL();

            var movie = vidlyDAL.Movies.SingleOrDefault(m => m.ID == id);

            if (movie == null)
                throw new HttpResponseException(HttpStatusCode.NotFound);

            return movie;
        }

        //POST /api/movies
        [HttpPost]
        public Movie CreateMovie(Movie movie)
        {
            if (!ModelState.IsValid)
                throw new HttpResponseException(HttpStatusCode.BadRequest);

            VidlyDAL vidlyDAL = new VidlyDAL();

            vidlyDAL.Movies.Add(movie);

            vidlyDAL.SaveChanges();

            return movie;
        }

        //PUT /api/movies/1
        [HttpPut]
        public void UpdateMovie(int id, Movie movie)
        {
            if (!ModelState.IsValid)
                throw new HttpResponseException(HttpStatusCode.BadRequest);

            VidlyDAL vidlyDAL = new VidlyDAL();

            var movieInDb = vidlyDAL.Movies.SingleOrDefault(m => m.ID == id);

            if (movieInDb == null)
                throw new HttpResponseException(HttpStatusCode.NotFound);

            movieInDb.Name = movie.Name;
            movieInDb.GenreID = movie.GenreID;
            movieInDb.ReleaseDate = movie.ReleaseDate;
            movieInDb.DateAdded = movie.DateAdded;
            movieInDb.NumberInStock = movie.NumberInStock;

            vidlyDAL.SaveChanges();
        }

        //DELETE /api/movies/1
        [HttpDelete]
        public void DeleteMovie(int id)
        {
            VidlyDAL vidlyDAL = new VidlyDAL();

            var movieInDb = vidlyDAL.Movies.SingleOrDefault(m => m.ID == id);

            if (movieInDb == null)
                throw new HttpResponseException(HttpStatusCode.NotFound);

            vidlyDAL.Movies.Remove(movieInDb);

            vidlyDAL.SaveChanges();
        }
    }
}

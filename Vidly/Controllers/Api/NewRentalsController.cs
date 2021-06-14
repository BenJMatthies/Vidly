using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Vidly.Dtos;
using Vidly.Models;

namespace Vidly.Controllers.Api
{
    public class NewRentalsController : ApiController
    {
        private ApplicationDbContext _context; 

        public NewRentalsController()
        {
            _context = new ApplicationDbContext();
        }

        public IHttpActionResult GetRental(int id)
        {
            var rental = _context.Rentals.SingleOrDefault(r => r.Id == id);

            if (rental == null)
                return NotFound();

            return Ok(rental);
        }

        [HttpPost]
        public IHttpActionResult CreateRental(NewRentalDto newRentalDto)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var customer = _context.Customers.SingleOrDefault(c => c.Id == newRentalDto.CustomerId);

            if (newRentalDto.MovieIds.Count == 0)
                return BadRequest("No Movie IDs provided.");

            if (customer == null)
                return BadRequest("Customer ID is not valid.");

            var movies = _context.Movies.Where(m => newRentalDto.MovieIds.Contains(m.Id)).ToList();

            if (movies.Count != newRentalDto.MovieIds.Count)
                return BadRequest("Some Movie IDs are invalid.");

            foreach (var movie in movies)
            {
                if (movie.NumberAvailable < 1)
                    return BadRequest("Movie is not available.");

                var today = DateTime.Now;
                movie.NumberAvailable--;

                var rental = new Rental
                {
                    Customer = customer,
                    Movie = movie,
                    DateRented = today,
                    DateDue = today.Add(new TimeSpan(7, 0, 0, 0))
                };

                _context.Rentals.Add(rental);
            }

            _context.SaveChanges();

            return Ok();
        }

        [HttpDelete]
        public void DeleteRental(int id)
        {
            var rentalInDb = _context.Rentals.SingleOrDefault(r => r.Id == id);

            if (rentalInDb == null)
                throw new HttpResponseException(HttpStatusCode.NotFound);

            _context.Rentals.Remove(rentalInDb);
            _context.SaveChanges();
        }
    }
}

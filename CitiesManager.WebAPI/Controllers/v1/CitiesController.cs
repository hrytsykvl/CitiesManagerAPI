using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CitiesManager.WebAPI.DatabaseContext;
using CitiesManager.WebAPI.Models;
using Microsoft.AspNetCore.Cors;

namespace CitiesManager.WebAPI.Controllers.v1
{
    [ApiVersion("1.0")]
    // [EnableCors("4100Client")]
    public class CitiesController : CustomControllerBase
    {
        private readonly ApplicationDbContext _context;

        public CitiesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Cities
        /// <summary>
        /// Get list of citites (including city ID and city name) from 'cities' table
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        // [Produces("application/xml")]
        public async Task<ActionResult<IEnumerable<City>>> GetCities()
        {
            return await _context.Cities
                .OrderBy(temp => temp.CityName).ToListAsync();
        }

        // GET: api/Cities/5
        /// <summary>
        /// Get city details by city id from 'cities' table
        /// </summary>
        /// <param name="id">City id to get</param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<City>> GetCity(Guid id)
        {
            var city = await _context.Cities.FirstOrDefaultAsync(city => city.CityID == id);

            if (city == null)
            {
                return Problem(detail: "Invalid CityID", statusCode: 400, title: "City Search");
                // return NotFound();
            }

            return city;
        }

        // PUT: api/Cities/5
        /// <summary>
        /// Update city name by city id in 'cities' table
        /// </summary>
        /// <param name="cityID">City id to update</param>
        /// <param name="city">City name to update</param>
        /// <returns></returns>
        [HttpPut("{cityID}")]
        public async Task<IActionResult> PutCity(Guid cityID,
            [Bind(nameof(City.CityID), nameof(City.CityName))] City city)
        {
            if (cityID != city.CityID)
            {
                return BadRequest();
            }

            var existingCity = await _context.Cities.FindAsync(cityID);
            if (existingCity == null)
            {
                return NotFound();
            }
            existingCity.CityName = city.CityName;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CityExists(cityID))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Cities
        /// <summary>
        /// Create and add new city into 'cities' table
        /// </summary>
        /// <param name="city">Created city object</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<City>> PostCity([Bind(nameof(City.CityID), nameof(City.CityName))] City city)
        {
            //if (ModelState.IsValid == false)
            //{
            //	return ValidationProblem(ModelState);
            //}

            if (_context.Cities == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Cities' is null.");
            }

            _context.Cities.Add(city);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetCity", new { id = city.CityID }, city);
        }

        // DELETE: api/Cities/5
        /// <summary>
        /// Delete city by city id from 'cities' table
        /// </summary>
        /// <param name="id">City id to delete</param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCity(Guid id)
        {
            var city = await _context.Cities.FindAsync(id);
            if (city == null)
            {
                return NotFound();
            }

            _context.Cities.Remove(city);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool CityExists(Guid id)
        {
            return _context.Cities.Any(e => e.CityID == id);
        }
    }
}

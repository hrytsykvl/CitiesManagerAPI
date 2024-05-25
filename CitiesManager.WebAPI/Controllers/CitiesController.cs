﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CitiesManager.WebAPI.DatabaseContext;
using CitiesManager.WebAPI.Models;

namespace CitiesManager.WebAPI.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class CitiesController : CustomControllerBase
	{
		private readonly ApplicationDbContext _context;

		public CitiesController(ApplicationDbContext context)
		{
			_context = context;
		}

		// GET: api/Cities
		[HttpGet]
		public async Task<ActionResult<IEnumerable<City>>> GetCities()
		{
			return await _context.Cities
				.OrderBy(temp => temp.CityName).ToListAsync();
		}

		// GET: api/Cities/5
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

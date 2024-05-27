﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CitiesManager.WebAPI.DatabaseContext;
using CitiesManager.WebAPI.Models;

namespace CitiesManager.WebAPI.Controllers.v2
{
	[ApiVersion("2.0")]
	[Area("api")]
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
		[Produces("application/xml")]
		public async Task<ActionResult<IEnumerable<string>>> GetCities()
		{
			return await _context.Cities
				.OrderBy(temp => temp.CityName)
				.Select(temp => temp.CityName)
				.ToListAsync();
		}
	}
}
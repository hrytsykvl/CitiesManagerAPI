using CitiesManager.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace CitiesManager.Infrastructure.DatabaseContext
{
	public class ApplicationDbContext : DbContext
	{
		public ApplicationDbContext(DbContextOptions options) : base(options)
		{
		}

		public ApplicationDbContext()
		{
		}

		public virtual DbSet<City> Cities { get; set; }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);

			modelBuilder.Entity<City>().HasData(new City()
			{
				CityID = Guid.Parse("9072C6B2-B43F-41C1-9B3B-BC248C92B457"),
				CityName = "Kyiv"
			});

			modelBuilder.Entity<City>().HasData(new City()
			{
				CityID = Guid.Parse("F3E6CCEF-006A-482B-BD25-B186C41824C6"),
				CityName = "Lviv"
			});
		}
	}
}

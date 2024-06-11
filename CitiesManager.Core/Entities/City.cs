using System.ComponentModel.DataAnnotations;

namespace CitiesManager.Core.Models
{
	public class City
	{
		[Key]
		public Guid CityID { get; set; }

		[Required(ErrorMessage = "City name is required")]
		public string? CityName { get; set; }
	}
}

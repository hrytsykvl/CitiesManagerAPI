using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace CitiesManager.Core.DTO
{
	public class RegisterDTO
	{
		[Required(ErrorMessage = "Person name can't be blank")]
		public string PersonName { get; set; }

		[Required(ErrorMessage = "Email can't be blank")]
		[EmailAddress(ErrorMessage = "Email should be in a proper email address format")]
		[Remote(action : "IsEmailAlreadyRegistered", controller: "Account", ErrorMessage = "Email is alredy being used")]
		public string Email { get; set; }

		[Required(ErrorMessage = "Phone number can't be blank")]
		[RegularExpression("^[0-9]*$", ErrorMessage = "Phone number should only contain digits")]
		[Remote(action: "IsEmailAlreadyRegistered", controller: "Account", ErrorMessage = "Email is alredy being used")]
		public string PhoneNumber { get; set; }

		[Required(ErrorMessage = "Password can't be blank")]
		public string Password { get; set; }

		[Required(ErrorMessage = "Confirm Password can't be blank")]
		[Compare("Password", ErrorMessage = "Confirm password should match with password")]
		public string ConfirmPassword { get; set; }
	}
}

using System.ComponentModel.DataAnnotations;

namespace ReactOAuthTest.Api.Dto
{
    public class AccountRegisterDto
    {
        [Required] [EmailAddress] public string Email { get; set; }

        [Required] public string FirstName { get; set; }

        [Required] public string LastName { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "PASSWORD_LENGTH", MinimumLength = 8)]
        public string Password { get; set; }
    }
}
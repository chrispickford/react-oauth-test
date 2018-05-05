using System.ComponentModel.DataAnnotations;

namespace ReactOAuthTest.Api.Dto
{
    public class AccountLoginDto
    {
        [Required] public string Email { get; set; }

        [Required] public string Password { get; set; }
    }
}
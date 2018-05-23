using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using ReactOAuthTest.Api.Dto;
using ReactOAuthTest.Api.Helpers;
using ReactOAuthTest.Data;
using ReactOAuthTest.Data.Entities;

namespace ReactOAuthTest.Api.Controllers
{
    [Authorize]
    [Produces("application/json")]
    [Route("api/[controller]")]
    public class AccountController : Controller
    {
        private readonly AppSettings _appSettings;
        private readonly SecurityContext _context;
        private readonly IMapper _mapper;
        private readonly SignInManager<User> _signInManager;
        private readonly UserManager<User> _userManager;

        public AccountController(UserManager<User> userManager, SignInManager<User> signInManager, IMapper mapper,
            IOptions<AppSettings> appSettings, SecurityContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _mapper = mapper;
            _context = context;
            _appSettings = appSettings.Value;
        }

        [HttpGet]
        public async Task<IActionResult> GetUserDetails()
        {
            var user = await _userManager.GetUserAsync(User);

            return Ok(new AccountUserDetailsDto
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email
            });
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] AccountLoginDto loginDto)
        {
            var signInResult =
                await _signInManager.PasswordSignInAsync(loginDto.Email, loginDto.Password, false, false);

            if (!signInResult.Succeeded)
                return Unauthorized();

            var user = await _userManager.Users.SingleAsync(x => x.Email == loginDto.Email);

            var accessToken = GenerateJwtToken(user.Id, user.Email);
            var refreshToken = await GenerateRefreshToken(user.Id);

            return Ok(new AccountTokenDto
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken.Token
            });
        }

        [AllowAnonymous]
        [HttpPost("refresh")]
        public async Task<IActionResult> Refresh(string refreshToken)
        {
            if (string.IsNullOrWhiteSpace(refreshToken)) return BadRequest();

            var token = await _context.RefreshTokens.Include(x => x.User)
                .SingleOrDefaultAsync(x => x.Token == refreshToken && x.ExpiresUtc > DateTime.Now);

            if (token == null) return BadRequest();

            var user = token.User;

            if (!await _signInManager.CanSignInAsync(user)) return Unauthorized();

            if (await _userManager.IsLockedOutAsync(user)) return Unauthorized();

            var newAccessToken = GenerateJwtToken(user.Id, user.Email);
            var newRefreshToken = await GenerateRefreshToken(user.Id);

            return Ok(new AccountTokenDto
            {
                AccessToken = newAccessToken,
                RefreshToken = newRefreshToken.Token
            });
        }

        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] AccountRegisterDto registerDto)
        {
            var user = _mapper.Map<User>(registerDto);

            var result = await _userManager.CreateAsync(user, registerDto.Password);

            if (!result.Succeeded)
                return BadRequest(result.Errors);

            await _signInManager.SignInAsync(user, false);

            var accessToken = GenerateJwtToken(user.Id, user.Email);
            var refreshToken = await GenerateRefreshToken(user.Id);

            return Ok(new AccountTokenDto
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken.Token
            });
        }

        private string GenerateJwtToken(string userId, string email)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.JwtKey);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(JwtRegisteredClaimNames.NameId, userId),
                    new Claim(JwtRegisteredClaimNames.Sub, email)
                }),
                Expires =
                    DateTime.UtcNow.AddMinutes(_appSettings
                        .JwtExpireMins), // TODO: Expire after inactivity - refresh token?
                SigningCredentials =
                    new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
                Audience = _appSettings.JwtAudience,
                Issuer = _appSettings.JwtIssuer
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }

        private async Task<RefreshToken> GenerateRefreshToken(string userId)
        {
            var token = new RefreshToken
            {
                UserId = userId,
                Token = Guid.NewGuid().ToString("N"),
                IssuedUtc = DateTime.UtcNow,
                ExpiresUtc = DateTime.Today.AddDays(1)
            };

            await _context.InsertOrUpdateRefreshToken(token);

            return token;
        }
    }
}
using HotelApp1.Entities.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;

namespace HotelApp1.Helpers
{
	public class TokenService
	{
		private readonly IConfiguration _configuration;
		private readonly HotelAppContext _context;

		public TokenService(IConfiguration configuration, HotelAppContext context )
		{
			_configuration = configuration;
			_context = context;
		}
		public string CreateToken(Client client)
		{
			List<Claim> claims = new List<Claim>
			{
				new Claim("ClientId", client.ClientId.ToString()),
				new Claim("username",client.Username),
				new Claim("name",client.Name),
				new Claim("role", client.Role)
			};

			var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8
				.GetBytes(_configuration.GetSection("AppSettings:Token").Value));
			//retrives the secret key from the application settings.
			//This key is used to sign the JWT.

			var creds = new SigningCredentials(key, SecurityAlgorithms
				.HmacSha256Signature);
			var token = new JwtSecurityToken(
				claims: claims,
				expires: DateTime.Now.AddDays(1),
				signingCredentials: creds);
			
			var jwt = new JwtSecurityTokenHandler().WriteToken(token);
			return jwt;

		}

		public static RefreshToken GenerateRefreshToken()
		{
			var refreshToken = new RefreshToken
			{

				Token = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64)),
				Expires = DateTime.Now.AddDays(7),
				Created = DateTime.Now
			};

			return refreshToken;
		}
		public void SetRefreshToken(HttpContext httpContext, Client client, RefreshToken refreshToken)
		{
			var cookieOptions = new CookieOptions
			{
				HttpOnly = true,
				Expires = refreshToken.Expires
			};
			httpContext.Response.Cookies.Append("refreshToken", refreshToken.Token, cookieOptions);

			_context.RefreshTokens.Add(refreshToken);
			_context.SaveChanges();
		}
	}
}

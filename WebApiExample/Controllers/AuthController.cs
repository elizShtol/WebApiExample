using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using WebApiExample.Models;

namespace WebApiExample.Controllers;

[ApiController]
[Route("auth")]
public class AuthController: ControllerBase
{
    private readonly JWTConfig jwtConfig;

    public AuthController(JWTConfig jwtConfig)
    {
        this.jwtConfig = jwtConfig;
    }

    [HttpPost(Name = "Authenticate")]
    public IActionResult Auth([FromBody] AuthRequest authRequest)
    {
        if (authRequest.Login == "login" && authRequest.Password == "password")
        {
            var key = Encoding.UTF8.GetBytes(jwtConfig.JwtKey);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim("Id", Guid.NewGuid().ToString()),
                    new Claim(JwtRegisteredClaimNames.Name, authRequest.Login),
                }),
                Expires = DateTime.UtcNow.AddMinutes(5),
                Issuer = jwtConfig.Issuer,
                Audience = jwtConfig.Audience,
                SigningCredentials = new SigningCredentials
                (new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha512Signature)
            };
            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var jwtToken = tokenHandler.WriteToken(token);
            return Ok(new AuthResponse{Token = jwtToken});
        }
        return Unauthorized();
    }
}
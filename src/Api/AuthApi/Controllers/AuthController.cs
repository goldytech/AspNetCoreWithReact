using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace AuthApi.Controllers;

[ApiController]
[Route("[controller]")]
public class AuthController : ControllerBase

{
    private readonly JwtSettings _config;

    public AuthController(IOptions<JwtSettings> config)
    {
        _config = config.Value;
    }
    [HttpPost]
    [Route("login")]
    public async Task<IActionResult> Login([FromBody] LoginModel loginModel)
    {
        if (loginModel is { Username: "admin", Password: "123" })
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, loginModel.Username),
                new Claim(ClaimTypes.Role, "admin"),
            };
            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var authProperties = new AuthenticationProperties
            {
                AllowRefresh = true,
                ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(60),
                IsPersistent = loginModel.RememberMe,
                IssuedUtc = DateTimeOffset.UtcNow,
            };
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), authProperties);
            return Ok();
        }
        return Unauthorized();
    }

    [HttpPost]
    [Route("login2")]
    public async Task<IActionResult> Login2([FromBody] TokenModel tokenModel)
    {
        if (tokenModel is { Username: "admin", Password: "123" })
        {
            var tokenResponse = new TokenResponse(GenerateToken(tokenModel));
            return Ok(tokenResponse);
        }

        return Unauthorized();
    }
    
    
    private string GenerateToken(TokenModel tokenModel)
    {
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config.Key));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier,tokenModel.Username),
            new Claim(ClaimTypes.Role,"admin")
        };
        var token = new JwtSecurityToken(_config.Issuer,
            tokenModel.Audience,
            claims,
            expires: DateTime.Now.AddMinutes(15),
            signingCredentials: credentials);


        return new JwtSecurityTokenHandler().WriteToken(token);

    }

}
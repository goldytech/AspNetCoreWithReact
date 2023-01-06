using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
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
        if (loginModel is not { Username: "admin", Password: "123" }) return Unauthorized();
        var claims = new List<Claim>
        {
            new(ClaimTypes.Name, loginModel.Username),
            new(ClaimTypes.Role, "admin"),
        };
        var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
        var authProperties = new AuthenticationProperties
        {
            AllowRefresh = true,
            ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(60),
            IsPersistent = loginModel.RememberMe,
            IssuedUtc = DateTimeOffset.UtcNow,
        };
        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
            new ClaimsPrincipal(claimsIdentity), authProperties);
        return Ok();
    }
    
    [HttpPost]
    [Route("logout")]
    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        return Ok();
    }

    [HttpPost]
    [Route("token")]
    public IActionResult Token([FromBody] TokenModel tokenModel)
    {
        if (tokenModel is not { ClientId: "admin", ClientSecret: "123" }) return Unauthorized();
        var tokenResponse = new TokenResponse(GenerateToken(tokenModel));
        return Ok(tokenResponse);

    }
    
      [HttpGet]
      [Route("userinfo")]
    public IActionResult GetUser()
    {
        return new JsonResult(User.Claims.Select(c => new { Type=c.Type, Value=c.Value }));
    }


    private string GenerateToken(TokenModel tokenModel)
    {
        var issuer = _config.Issuer;
        var audience = tokenModel.Audience;
        var key = Encoding.ASCII.GetBytes
            (_config.Key);
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[]
            {
                new Claim("Id", Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Sub, tokenModel.ClientId),
                new Claim(JwtRegisteredClaimNames.Name, tokenModel.ClientId),
                new Claim(JwtRegisteredClaimNames.Jti,
                    Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.Role, "admin"),
            }),
            Expires = DateTime.UtcNow.AddMinutes(60),
            Issuer = issuer,
            Audience = audience,
            SigningCredentials = new SigningCredentials
            (new SymmetricSecurityKey(key),
                SecurityAlgorithms.HmacSha512Signature)
        };
        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(tokenDescriptor);
        var jwtToken = tokenHandler.WriteToken(token);
        var stringToken = tokenHandler.WriteToken(token);
        return stringToken;
    }
}


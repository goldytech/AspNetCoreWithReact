using System.Text;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection(nameof(JwtSettings)));
builder.Services.AddAuthentication().AddCookie(CookieAuthenticationDefaults.AuthenticationScheme);
// builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options => {
//     options.TokenValidationParameters = new TokenValidationParameters {
//         ValidateIssuer = true,
//         ValidateAudience = true,
//         ValidateLifetime = true,
//         ValidateIssuerSigningKey = true,
//         ValidIssuer = builder.Configuration["JwtSettings:Issuer"],
//         //ValidAudience = builder.Configuration["Jwt:Audience"],
//         IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JwtSettings:Key"] ?? string.Empty))
//     };
// });

builder.Services.AddControllers();
builder.Services.AddCors(options =>
{
    options.AddPolicy("react-app", builder =>
    {
        builder.WithOrigins("http://localhost:3001")
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials(); // Credentials should be allowed so that cookie can be transferred with every request
    });
});
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


//app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.UseCors("react-app");
app.MapControllers();

app.Run();

public record LoginModel(string Username, string Password, bool RememberMe);
public record TokenModel(string ClientId, string ClientSecret, string Audience);

public class JwtSettings
{
    public string Key { get; set; }
    public string Issuer { get; set; }
}

public record TokenResponse(string AccessToken);


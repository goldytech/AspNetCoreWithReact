using System.Text;
using CustomerApi.Common.Logging;
using CustomerApi.Core.Validation;
using CustomerApi.Domain.Customers;
using CustomerApi.Domain.Customers.CreateCustomer;
using CustomerApi.Domain.Customers.GetAllCustomers;
using CustomerApi.Domain.Customers.GetSingleCustomer;
using CustomerApi.Domain.Customers.UpdateSingleCustomer;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(o =>
{
    o.TokenValidationParameters = new TokenValidationParameters
    {
        ValidIssuer = builder.Configuration["JwtSettings:Issuer"],
        ValidAudience = builder.Configuration["JwtSettings:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey
            (Encoding.UTF8.GetBytes(builder.Configuration["JwtSettings:Key"])),
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = false,
        ValidateIssuerSigningKey = true
    };
});
builder.Services.AddAuthorization();



builder.Services.AddCustomerServices()
                .AddMongoDatabase(builder)
                .AddLoggingInfrastructure(builder);
    

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI();
}
// Make the APIs secure by injecting the middleware
app.UseAuthentication();
app.UseAuthorization();
// This is for testing purpose, should not be enabled in production
//app.UseCors(policyBuilder => policyBuilder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader()); 
var v1 = app.MapGroup("/api/v1");

v1.MapPost("/customers", CreateCustomerEndpoint.CreateCustomer)
    .AddEndpointFilterFactory(ValidationFilter.ValidationFilterFactory)
    .Produces(StatusCodes.Status201Created)
    .Produces(StatusCodes.Status401Unauthorized)
    .Produces(StatusCodes.Status400BadRequest)
    .RequireAuthorization()
    .Produces(StatusCodes.Status500InternalServerError);

v1.MapGet("/customers/{customerId}", GetSingleCustomerEndpoint.GetById)
    .Produces(StatusCodes.Status404NotFound)
    .Produces(StatusCodes.Status401Unauthorized)
    .Produces(StatusCodes.Status500InternalServerError)
    .Produces<SingleCustomerResponseDto>(StatusCodes.Status200OK)
    .WithName("GetById")
    .RequireAuthorization()
    .WithDisplayName("Get By Customer Id");

v1.MapGet("/customers", GetAllCustomersEndpoint.GetAll)
    .RequireAuthorization()
    .Produces(StatusCodes.Status500InternalServerError)
    .Produces<List<GetAllCustomersResponseDto>>(StatusCodes.Status200OK)
    .Produces(StatusCodes.Status401Unauthorized)
    .WithName("GetAll")
    .WithDisplayName("Get All Customers");

v1.MapPut("/customers/{customerId}", UpdateSingleCustomerEndpoint.UpdateCustomer)
    .Produces(StatusCodes.Status404NotFound)
    .Produces(StatusCodes.Status401Unauthorized)
    .Produces(StatusCodes.Status500InternalServerError)
    .Produces(StatusCodes.Status204NoContent)
    .WithName("UpdateCustomer")
    .WithDisplayName("Update Customer");

app.Run();
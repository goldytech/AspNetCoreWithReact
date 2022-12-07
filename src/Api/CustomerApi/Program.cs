using CustomerApi.Common.Logging;
using CustomerApi.Common.Models;
using CustomerApi.Core;
using CustomerApi.Core.Validation;
using CustomerApi.Domain.Customers;
using CustomerApi.Domain.Customers.CreateCustomer;
using CustomerApi.Domain.Customers.GetSingleCustomer;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();



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

var v1 = app.MapGroup("/api/v1");

v1.MapPost("/customers", CreateCustomerEndpoint.CreateCustomer)
    .AddEndpointFilterFactory(ValidationFilter.ValidationFilterFactory)
    .Produces(StatusCodes.Status201Created)
    .Produces(StatusCodes.Status400BadRequest)
    .Produces(StatusCodes.Status500InternalServerError);

v1.MapGet("/customers/{customerId}", GetSingleCustomerEndpoint.GetById)
    .Produces(StatusCodes.Status404NotFound)
    .Produces(StatusCodes.Status500InternalServerError)
    .Produces<SingleCustomerResponseModel>(StatusCodes.Status200OK)
    .WithName("GetById")
    .WithDisplayName("Get By Customer Id");

app.Run();
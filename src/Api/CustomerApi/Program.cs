using CustomerApi.Core.Validation;
using CustomerApi.Domain.Customers;
using CustomerApi.Domain.Customers.CreateCustomer;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCustomerServices();

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
    .AddEndpointFilterFactory(ValidationFilter.ValidationFilterFactory);
app.Run();
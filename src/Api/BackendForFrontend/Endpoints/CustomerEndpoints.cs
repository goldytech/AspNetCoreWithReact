using System.Net;
using System.Net.Http.Headers;
using BackendForFrontend.Dto;
using Dapr;
using Dapr.Client;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Models;

namespace BackendForFrontend.Endpoints;

internal static class CustomerEndpoints
{
    public static RouteGroupBuilder MapCustomers(this IEndpointRouteBuilder endpoints)
    {
        string[] emails =
        {
            "joe@gmail.com", "john@outlook.com", "marry@hotmail.com"
        };
        var customersGroup = endpoints.MapGroup("/customers");
        customersGroup.RequireAuthorization();
        customersGroup.MapGet("/", async ([FromServices] ILoggerFactory loggerFactory,
                [FromServices] ITokenService tokenService,
                [FromServices] DaprClient daprClient) =>
            {
                var logger = loggerFactory.CreateLogger("CustomerEndpoints");
                try
                {
                    var request = daprClient.CreateInvokeMethodRequest(HttpMethod.Get, "customers-api",
                        "api/v1/customers");
                    var token = await tokenService.GetJwtTokenForApi2("customers-api", daprClient);
                    if (token is null)
                    {
                        // Was unable to retrieve token. Hence returning 500 response.
                        return Results.Problem("Failed to get token for customers-api");
                    }

                    request.Headers.Authorization = new AuthenticationHeaderValue("Bearer",
                        token);

                    var customers = await daprClient.InvokeMethodAsync<IEnumerable<GetAllCustomersResponseDto>>
                        (request);

                    var customersList = customers
                        .Select(dto => new Customer(dto.Id, dto.Name, emails[Random.Shared.Next(emails.Length)]))
                        .ToList();

                    return Results.Ok(customersList);
                }
                catch (DaprException e)
                {
                    logger.LogError(e, "Dapr Exception : Error while getting customers");
                    return Results.Problem("Error while getting customers");
                }
                catch (Exception e)
                {
                    logger.LogError(e, "General Exception : Error while getting customers");
                    return Results.Problem("Error while getting customers");
                }
            }).WithName("GetCustomers")
            .WithDescription("Get all customers list")
            .WithOpenApi(operation =>
            {
                operation.Security = new List<OpenApiSecurityRequirement>
                {
                    GetDefaultSecurityRequirement()
                };
                return operation;
            });
        

        customersGroup.MapPost("/", async ([FromServices] ILoggerFactory loggerFactory,
            [FromBody] CreateCustomerRequestDto requestDto,
            [FromServices] ITokenService tokenService,
            [FromServices] DaprClient daprClient) =>
        {
            var logger = loggerFactory.CreateLogger("CustomerEndpoints");
            try
            {
               // invoking the service to service communication with Dapr Client using HttpClient method #2
                var httpClient = DaprClient.CreateInvokeHttpClient("customers-api");
                var token = await tokenService.GetJwtTokenForApi2("customers-api", daprClient);
                if (token is null)
                {
                    // Was unable to retrieve token. Hence returning 500 response.
                    return Results.Problem("Failed to get token for customers-api");
                }

                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer",
                    token);
                var response = await httpClient.PostAsJsonAsync("api/v1/customers", requestDto);

                switch (response.StatusCode)
                {
                    case HttpStatusCode.Created when response.Headers.Location != null:
                        return Results.Created(response.Headers.Location,
                            await response.Content.ReadFromJsonAsync<string>());
                    case HttpStatusCode.UnprocessableEntity:
                    {
                        var errorResponse = await response.Content.ReadFromJsonAsync<ValidationProblemDetails>();
                        return Results.BadRequest(errorResponse);
                    }
                    case HttpStatusCode.InternalServerError:
                    {
                        var errorResponse = await response.Content.ReadFromJsonAsync<ProblemDetails>();
                        return Results.Problem(errorResponse?.Detail);
                    }
                    default:
                        return Results.Empty;
                }
            }
            catch (DaprException e)
            {
                logger.LogError(e, "Dapr Exception : Error while creating customer");
                return Results.Problem("Error while creating customer");
            }
            catch (Exception e)
            {
                logger.LogError(e, "General Exception : Error while creating customer");
                return Results.Problem("Error while getting customer");
            }
        }).WithName("CreateCustomer")
            .WithDescription("Create a new Customer")
            .WithOpenApi(operation =>
            {
                operation.Security = new List<OpenApiSecurityRequirement>
                {
                    GetDefaultSecurityRequirement()
                };
                return operation;
            });
        customersGroup.MapGet("/{id}", async ([FromServices] ILoggerFactory loggerFactory,
            [FromRoute] string id,
            [FromServices] ITokenService tokenService,
            [FromServices] DaprClient daprClient) =>
        {
            var logger = loggerFactory.CreateLogger("CustomerEndpoints");
            try
            {
                // method #3 of invoking the service to service communication with Dapr Client
                var request = daprClient.CreateInvokeMethodRequest(HttpMethod.Get, "customers-api",
                    $"api/v1/customers/{id}");
                var token = await tokenService.GetJwtTokenForApi2("customers-api", daprClient);
                if (token is null)
                {
                    // Was unable to retrieve token. Hence returning 500 response.
                    return Results.Problem("Failed to get token for customers-api");
                }

                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

                var customer = await daprClient.InvokeMethodAsync<SingleCustomerResponseDto>(request);

                return Results.Ok(new Customer("101", customer.Name, emails[Random.Shared.Next(emails.Length)]));
            }
            catch (DaprException e)
            {
                logger.LogError(e, "Dapr Exception : Error while getting customer");
                return Results.Problem("Error while getting customer");
            }
            catch (Exception e)
            {
                logger.LogError(e, "General Exception : Error while getting customer");
                return Results.Problem("Error while getting customer");
            }
        });
        return customersGroup;
    }

    private static OpenApiSecurityRequirement GetDefaultSecurityRequirement()
    {
        return new OpenApiSecurityRequirement
        {
            {
                new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = JwtBearerDefaults.AuthenticationScheme
                    },
                    Scheme = SecuritySchemeType.Http.ToString(),
                    Name = JwtBearerDefaults.AuthenticationScheme
                },
                new List<string>()


            }
        };
    }


    

}
using CustomerApi.Common.Models;
using CustomerApi.Common.MongoDbServices;
using FluentValidation;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace CustomerApi.Domain.Customers;

public static class CustomerServicesRegistration
{
    public static IServiceCollection AddCustomerServices(this IServiceCollection services)
    {
        services.AddValidatorsFromAssemblyContaining<Program>(ServiceLifetime.Singleton);
        services.AddScoped<ICustomerService, CustomerService>();
        return services;
    }
    public static IServiceCollection AddMongoDatabase(this IServiceCollection services, WebApplicationBuilder builder)
    {
        services.Configure<MongoDbSettings>(builder.Configuration.GetSection(nameof(MongoDbSettings)));
        services.AddSingleton<IMongoDbSettings>(serviceProvider =>
            serviceProvider.GetRequiredService<IOptions<MongoDbSettings>>().Value);

         services.AddSingleton<IMongoClient>(serviceProvider =>
         {
             var settings = serviceProvider.GetRequiredService<IOptions<MongoDbSettings>>().Value;
             return new MongoClient(MongoClientSettings.FromConnectionString(settings.ConnectionString));
         });
// //   The lifetime of the IMongoDatabase interface should be scoped, meaning that a new instance of the database will be
// //   created for each request. This is because the database connection is not thread-safe and must be used within the same request.
//   // If the IMongoDatabase interface is registered with a singleton lifetime, only one instance of the database will
//   // be created and shared among all requests, which can lead to threading issues and potential data corruption.
//

         services.AddScoped<IMongoDatabase>(serviceProvider =>
         {
             var settings = serviceProvider.GetRequiredService<IOptions<MongoDbSettings>>().Value;
             var client = serviceProvider.GetRequiredService<IMongoClient>();
             return client.GetDatabase(settings.DatabaseName);
         });
         //services.AddScoped<IRepository<T>, MongoRepository<T>>();
         services.AddScoped(typeof(IRepository<>), typeof(MongoRepository<>));

        return services;
       
    }
}
using FluentValidation;

namespace CustomerApi.Domain.Customers;

public static class CustomerServicesRegistration
{
    public static IServiceCollection AddCustomerServices(this IServiceCollection services)
    {
        services.AddValidatorsFromAssemblyContaining<Program>(ServiceLifetime.Singleton);
        services.AddScoped<ICustomerService, CustomerService>();
        return services;
    }
}
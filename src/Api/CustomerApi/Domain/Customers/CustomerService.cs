using CustomerApi.Common.MongoDbServices;
using CustomerApi.Core;
using CustomerApi.Domain.Customers.CreateCustomer;
using CustomerApi.Domain.Customers.GetAllCustomers;
using CustomerApi.Domain.Customers.GetSingleCustomer;
using CustomerApi.Domain.Customers.UpdateSingleCustomer;

namespace CustomerApi.Domain.Customers;

public class CustomerService : ICustomerService
{
    private readonly IRepository<CustomerEntity> _customerRepository;
    private readonly ILogger<CustomerService> _logger;

    public CustomerService(IRepository<CustomerEntity> customerRepository, ILogger<CustomerService> logger)

    {
        _customerRepository = customerRepository;
        _logger = logger;
    }

    public async Task<Result<string, Exception>?> CreateCustomerAsync(CreateCustomerRequestDto customer)
    {
        try
        {
            var customerEntity = new CustomerEntity
            {
                CustomerId = customer.CustomerId,
                Name = customer.Name,
                Address = customer.Address,
            };

            await _customerRepository.InsertOneAsync(customerEntity);
            return Result<string, Exception>.SucceedWith(customerEntity.Id.ToString());
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error while creating customer");
            return Result<string, Exception>.FailWith(e);
        }
    }

    public Task<Result<SingleCustomerResponseDto, Exception>> GetCustomerByIdAsync(string id)
    {
        try
        {
            var customer = _customerRepository.FilterBy(x => x.Id.Equals(id), c =>
                new SingleCustomerResponseDto { Name = c.Name, Address = c.Address });
            return Task.FromResult(
                Result<SingleCustomerResponseDto, Exception>.SucceedWith(customer.FirstOrDefault() ??
                                                                           new SingleCustomerResponseDto()));
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error while getting customer");
            return Task.FromResult(Result<SingleCustomerResponseDto, Exception>.FailWith(e));
        }
    }

    public Task<Result<IEnumerable<GetAllCustomersResponseDto>, Exception>> GetAllCustomersAsync()
    {
        try
        {
            var customers = _customerRepository.AsQueryable()
                .Select(customerEntity => new GetAllCustomersResponseDto
                {   
                    Name = customerEntity.Name, 
                    Id = customerEntity.Id.ToString(), 
                    CustomerId = customerEntity.CustomerId
                    
                }).ToList();
            
            return Task.FromResult(Result<IEnumerable<GetAllCustomersResponseDto>, Exception>.SucceedWith(customers));
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error while getting all customers");
            return Task.FromResult(Result<IEnumerable<GetAllCustomersResponseDto>, Exception>.FailWith(e));
        }
    }

    public async Task<Result<bool?, Exception>> UpdateCustomerAsync(string id, UpdateSingleCustomerRequestDto updateSingleCustomerRequestDto)
    {
        try
        {
            
            var customer = await _customerRepository.FindOneAsync(x => x.Id.Equals(id));

            if (string.IsNullOrEmpty(customer.Id.ToString()))
            {
                return Result<bool?, Exception>.SucceedWith(null);
            }
            customer.Name = updateSingleCustomerRequestDto.Name;
            customer.Address.City = updateSingleCustomerRequestDto.City;
            await _customerRepository.ReplaceOneAsync(customer);
            return Result<bool?, Exception>.SucceedWith(true);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error while updating customer");
            return Result<bool?, Exception>.FailWith(e);
        }
        
    }
}


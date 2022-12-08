using CustomerApi.Common.Models;
using CustomerApi.Core;
using CustomerApi.Domain.Customers;
using CustomerApi.Domain.Customers.GetSingleCustomer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Moq;

namespace CustomerApi.Tests;

public class GetByIdEndpointTest
{
    [Fact]
    public async Task Given_When_Id_Not_Present_in_Db_Then_It_Should_Return_NotFound()
    {
        // ARRANGE
        var mockCustomerService = new Mock<ICustomerService>();
        Result<SingleCustomerResponseDto, Exception> result = Result<SingleCustomerResponseDto,Exception>.SucceedWith(new SingleCustomerResponseDto());
        mockCustomerService.Setup(x => x.GetCustomerByIdAsync(It.IsAny<string>()))
            .ReturnsAsync(result);
        
        // ACT
        var endpoint = (NotFound) await GetSingleCustomerEndpoint.GetById(It.IsAny<string>(),mockCustomerService.Object);
        
        // ASSERT
        Assert.Equal(StatusCodes.Status404NotFound, endpoint.StatusCode);
    }
    
    [Fact]
    public async Task Given_When_Id_Is_Present_in_Db_Then_It_Should_Return_Ok()
    {
        // ARRANGE
        var mockCustomerService = new Mock<ICustomerService>();
        Result<SingleCustomerResponseDto, Exception> result = Result<SingleCustomerResponseDto,Exception>.SucceedWith(new SingleCustomerResponseDto
            {Address = new Address{City = "TestCity",  Street = "TestStreet", Zip = "TestZipCode",State = "TestState"}, 
                 Name = "TestName"});
        mockCustomerService.Setup(x => x.GetCustomerByIdAsync(It.IsAny<string>()))
            .ReturnsAsync(result);
        
        // ACT
        var endpoint = (Ok<SingleCustomerResponseDto>) await GetSingleCustomerEndpoint.GetById(It.IsAny<string>(),mockCustomerService.Object);
        
        // ASSERT
        Assert.Equal(StatusCodes.Status200OK, endpoint.StatusCode);
    }
}
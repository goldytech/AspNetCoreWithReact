using CustomerApi.Common.Validators;
using FluentValidation;

namespace CustomerApi.Domain.Customers.CreateCustomer;

public class CreateCustomerRequestValidator : AbstractValidator<CreateCustomerRequestModel>
{
    public CreateCustomerRequestValidator()
    {
        
        RuleFor(x => x.Name).NotEmpty().WithMessage("Name is required")
            .MaximumLength(20). WithMessage("Name must be less than 20 characters");
        RuleFor(x => x.Address).SetValidator(new AddressValidator());
    }
}
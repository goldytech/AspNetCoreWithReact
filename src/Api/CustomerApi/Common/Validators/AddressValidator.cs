using CustomerApi.Common.Models;
using FluentValidation;

namespace CustomerApi.Common.Validators;

public class AddressValidator : AbstractValidator<Address>
{
    public AddressValidator()
    {
        RuleFor(x => x.Street).NotEmpty().WithMessage("Street is required");
        RuleFor(x => x.City).NotEmpty().WithMessage("City is required");
        RuleFor(x => x.State).NotEmpty().WithMessage("State is required");
        RuleFor(x => x.Zip).NotEmpty().WithMessage("ZipCode is required");
    }
}
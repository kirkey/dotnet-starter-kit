using FluentValidation;

namespace FSH.Starter.WebApi.Warehouse.Features.Customers.Create.v1;

public sealed class CreateCustomerValidator : AbstractValidator<CreateCustomerCommand>
{
    public CreateCustomerValidator()
    {
        RuleFor(p => p.FirstName).NotEmpty();
        RuleFor(p => p.LastName).NotEmpty();
        RuleFor(p => p.Email).EmailAddress().When(x => !string.IsNullOrWhiteSpace(x.Email));
    }
}

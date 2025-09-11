using FluentValidation;

namespace FSH.Starter.WebApi.Warehouse.Features.Customers.Update.v1;

public sealed class UpdateCustomerValidator : AbstractValidator<UpdateCustomerCommand>
{
    public UpdateCustomerValidator()
    {
        RuleFor(p => p.Id).NotEmpty();
    }
}


namespace FSH.Starter.WebApi.Store.Application.Customers.Activate.v1;

public sealed class ActivateCustomerCommandValidator : AbstractValidator<ActivateCustomerCommand>
{
    public ActivateCustomerCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}


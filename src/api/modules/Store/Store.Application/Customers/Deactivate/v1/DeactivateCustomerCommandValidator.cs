namespace FSH.Starter.WebApi.Store.Application.Customers.Deactivate.v1;

public sealed class DeactivateCustomerCommandValidator : AbstractValidator<DeactivateCustomerCommand>
{
    public DeactivateCustomerCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}


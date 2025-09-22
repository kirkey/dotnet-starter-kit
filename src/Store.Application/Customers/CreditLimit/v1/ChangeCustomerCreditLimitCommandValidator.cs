namespace FSH.Starter.WebApi.Store.Application.Customers.CreditLimit.v1;

public sealed class ChangeCustomerCreditLimitCommandValidator : AbstractValidator<ChangeCustomerCreditLimitCommand>
{
    public ChangeCustomerCreditLimitCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.NewCreditLimit).GreaterThanOrEqualTo(0);
    }
}


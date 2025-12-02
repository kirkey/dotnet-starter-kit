namespace Accounting.Application.Accruals.Create;

public sealed class CreateAccrualValidator : AbstractValidator<CreateAccrualCommand>
{
    public CreateAccrualValidator()
    {
        RuleFor(x => x.AccrualNumber)
            .NotEmpty()
            .MaximumLength(64);
        RuleFor(x => x.AccrualDate)
            .NotEmpty();
        RuleFor(x => x.Amount)
            .GreaterThan(0m);
        RuleFor(x => x.Description)
            .MaximumLength(256)
            .When(x => !string.IsNullOrWhiteSpace(x.Description));
    }
}


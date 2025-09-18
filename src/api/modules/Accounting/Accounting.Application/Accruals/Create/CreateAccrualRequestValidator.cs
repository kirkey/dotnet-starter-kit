namespace Accounting.Application.Accruals.Create;

public class CreateAccrualRequestValidator : AbstractValidator<CreateAccrualRequest>
{
    public CreateAccrualRequestValidator()
    {
        RuleFor(x => x.AccrualNumber)
            .NotEmpty()
            .MaximumLength(128);

        RuleFor(x => x.AccrualDate)
            .NotEmpty();

        RuleFor(x => x.Amount)
            .GreaterThan(0);

        RuleFor(x => x.Description)
            .MaximumLength(1000)
            .When(x => !string.IsNullOrEmpty(x.Description));
    }
}


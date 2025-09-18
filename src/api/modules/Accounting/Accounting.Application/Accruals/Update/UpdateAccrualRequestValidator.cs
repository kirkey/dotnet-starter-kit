namespace Accounting.Application.Accruals.Update;

public class UpdateAccrualRequestValidator : AbstractValidator<UpdateAccrualRequest>
{
    public UpdateAccrualRequestValidator()
    {
        RuleFor(x => x.Id).NotEmpty();

        RuleFor(x => x.AccrualNumber)
            .MaximumLength(128)
            .When(x => !string.IsNullOrEmpty(x.AccrualNumber));

        RuleFor(x => x.Amount)
            .GreaterThan(0)
            .When(x => x.Amount.HasValue);

        RuleFor(x => x.Description)
            .MaximumLength(1000)
            .When(x => !string.IsNullOrEmpty(x.Description));
    }
}


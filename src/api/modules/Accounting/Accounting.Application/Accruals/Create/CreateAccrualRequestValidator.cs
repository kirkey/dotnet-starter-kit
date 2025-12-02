namespace Accounting.Application.Accruals.Create;

public sealed class CreateAccrualCommandValidator : AbstractValidator<CreateAccrualCommand>
{
    public CreateAccrualCommandValidator()
    {
        RuleFor(x => x.AccrualNumber)
            .NotEmpty()
            .MaximumLength(64);

        RuleFor(x => x.AccrualDate)
            .NotEmpty();

        RuleFor(x => x.Amount)
            .GreaterThan(0);

        RuleFor(x => x.Description)
            .MaximumLength(256);
    }
}

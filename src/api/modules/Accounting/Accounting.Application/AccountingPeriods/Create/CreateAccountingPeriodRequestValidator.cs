using FluentValidation;

namespace Accounting.Application.AccountingPeriods.Create;

public class CreateAccountingPeriodRequestValidator : AbstractValidator<CreateAccountingPeriodRequest>
{
    public CreateAccountingPeriodRequestValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(256);

        RuleFor(x => x.StartDate)
            .NotEmpty();

        RuleFor(x => x.EndDate)
            .NotEmpty()
            .GreaterThan(x => x.StartDate);

        RuleFor(x => x.FiscalYear)
            .GreaterThan(1900)
            .LessThanOrEqualTo(2100);

        RuleFor(x => x.PeriodType)
            .NotEmpty()
            .MaximumLength(16);

        RuleFor(x => x.Description)
            .MaximumLength(1000)
            .When(x => !string.IsNullOrEmpty(x.Description));

        RuleFor(x => x.Notes)
            .MaximumLength(1000)
            .When(x => !string.IsNullOrEmpty(x.Notes));
    }
}

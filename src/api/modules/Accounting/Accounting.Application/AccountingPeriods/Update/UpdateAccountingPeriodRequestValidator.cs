namespace Accounting.Application.AccountingPeriods.Update;

public class UpdateAccountingPeriodRequestValidator : AbstractValidator<UpdateAccountingPeriodRequest>
{
    public UpdateAccountingPeriodRequestValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty();

        RuleFor(x => x.Name)
            .MaximumLength(256)
            .When(x => !string.IsNullOrEmpty(x.Name));

        RuleFor(x => x.EndDate)
            .GreaterThan(x => x.StartDate)
            .When(x => x.StartDate.HasValue && x.EndDate.HasValue);

        RuleFor(x => x.PeriodType)
            .MaximumLength(16)
            .When(x => !string.IsNullOrEmpty(x.PeriodType));

        RuleFor(x => x.Description)
            .MaximumLength(1000)
            .When(x => !string.IsNullOrEmpty(x.Description));

        RuleFor(x => x.Notes)
            .MaximumLength(1000)
            .When(x => !string.IsNullOrEmpty(x.Notes));
    }
}

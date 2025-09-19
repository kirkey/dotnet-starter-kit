namespace Accounting.Application.AccountingPeriods.Update.v1;

/// <summary>
/// Validator for <see cref="UpdateAccountingPeriodCommand"/> ensuring provided updates are valid before applying them.
/// </summary>
public class UpdateAccountingPeriodCommandValidator : AbstractValidator<UpdateAccountingPeriodCommand>
{
    public UpdateAccountingPeriodCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty();

        RuleFor(x => x.Name)
            .MaximumLength(1024)
            .When(x => !string.IsNullOrEmpty(x.Name));

        RuleFor(x => x.EndDate)
            .GreaterThan(x => x.StartDate!.Value)
            .When(x => x.StartDate.HasValue && x.EndDate.HasValue);

        RuleFor(x => x.FiscalYear)
            .GreaterThan(1899)
            .LessThanOrEqualTo(2100)
            .When(x => x.FiscalYear.HasValue);

        RuleFor(x => x.PeriodType)
            .MaximumLength(16)
            .When(x => !string.IsNullOrEmpty(x.PeriodType));

        RuleFor(x => x.Description)
            .MaximumLength(2048)
            .When(x => !string.IsNullOrEmpty(x.Description));

        RuleFor(x => x.Notes)
            .MaximumLength(2048)
            .When(x => !string.IsNullOrEmpty(x.Notes));
    }
}

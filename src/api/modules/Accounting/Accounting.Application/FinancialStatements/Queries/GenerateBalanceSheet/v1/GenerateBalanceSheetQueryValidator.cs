using Accounting.Application.FinancialStatements.Queries.GenerateBalanceSheet.v1;
using FluentValidation;

namespace Accounting.Application.FinancialStatements.Queries.GenerateBalanceSheet.v1;

public class GenerateBalanceSheetQueryValidator : AbstractValidator<GenerateBalanceSheetQuery>
{
    public GenerateBalanceSheetQueryValidator()
    {
        RuleFor(x => x.AsOfDate)
            .NotEmpty()
            .WithMessage("As of date is required")
            .LessThanOrEqualTo(DateTime.Today)
            .WithMessage("As of date cannot be in the future");

        RuleFor(x => x.ReportFormat)
            .NotEmpty()
            .WithMessage("Report format is required")
            .Must(x => new[] { "Standard", "Detailed", "Summary" }.Contains(x))
            .WithMessage("Report format must be Standard, Detailed, or Summary");

        When(x => x.IncludeComparativePeriod, () =>
        {
            RuleFor(x => x.ComparativeAsOfDate)
                .NotEmpty()
                .WithMessage("Comparative as of date is required when including comparative period")
                .LessThan(x => x.AsOfDate)
                .WithMessage("Comparative as of date must be before the main as of date");
        });
    }
}

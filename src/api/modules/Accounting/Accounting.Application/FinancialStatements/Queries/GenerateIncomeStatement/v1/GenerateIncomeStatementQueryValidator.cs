using Accounting.Application.FinancialStatements.Queries.GenerateIncomeStatement.v1;
using FluentValidation;

namespace Accounting.Application.FinancialStatements.Queries.GenerateIncomeStatement.v1;

public class GenerateIncomeStatementQueryValidator : AbstractValidator<GenerateIncomeStatementQuery>
{
    public GenerateIncomeStatementQueryValidator()
    {
        RuleFor(x => x.StartDate)
            .NotEmpty()
            .WithMessage("Start date is required")
            .LessThanOrEqualTo(DateTime.Today)
            .WithMessage("Start date cannot be in the future");

        RuleFor(x => x.EndDate)
            .NotEmpty()
            .WithMessage("End date is required")
            .GreaterThanOrEqualTo(x => x.StartDate)
            .WithMessage("End date must be greater than or equal to start date")
            .LessThanOrEqualTo(DateTime.Today)
            .WithMessage("End date cannot be in the future");

        RuleFor(x => x.ReportFormat)
            .NotEmpty()
            .WithMessage("Report format is required")
            .Must(x => new[] { "Standard", "Detailed", "Summary" }.Contains(x))
            .WithMessage("Report format must be Standard, Detailed, or Summary");

        When(x => x.IncludeComparativePeriod, () =>
        {
            RuleFor(x => x.ComparativeStartDate)
                .NotEmpty()
                .WithMessage("Comparative start date is required when including comparative period");

            RuleFor(x => x.ComparativeEndDate)
                .NotEmpty()
                .WithMessage("Comparative end date is required when including comparative period")
                .GreaterThanOrEqualTo(x => x.ComparativeStartDate)
                .WithMessage("Comparative end date must be greater than or equal to comparative start date");
        });
    }
}

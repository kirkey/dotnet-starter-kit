using FluentValidation;

namespace Accounting.Application.FinancialStatements.Queries.GenerateCashFlowStatement.v1;

public class GenerateCashFlowStatementQueryValidator : AbstractValidator<GenerateCashFlowStatementQuery>
{
    public GenerateCashFlowStatementQueryValidator()
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

        RuleFor(x => x.Method)
            .NotEmpty()
            .WithMessage("Method is required")
            .Must(x => new[] { "Direct", "Indirect" }.Contains(x))
            .WithMessage("Method must be Direct or Indirect");

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

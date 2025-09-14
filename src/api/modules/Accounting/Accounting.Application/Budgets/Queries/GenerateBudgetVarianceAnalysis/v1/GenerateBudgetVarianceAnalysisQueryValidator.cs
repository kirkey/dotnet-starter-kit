namespace Accounting.Application.Budgets.Queries.GenerateBudgetVarianceAnalysis.v1;

public class GenerateBudgetVarianceAnalysisQueryValidator : AbstractValidator<GenerateBudgetVarianceAnalysisQuery>
{
    public GenerateBudgetVarianceAnalysisQueryValidator()
    {
        RuleFor(x => x.BudgetId)
            .NotEmpty()
            .WithMessage("Budget ID is required");

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

        RuleFor(x => x.AnalysisType)
            .NotEmpty()
            .WithMessage("Analysis type is required")
            .Must(x => new[] { "Summary", "Detailed", "ByCategory" }.Contains(x))
            .WithMessage("Analysis type must be Summary, Detailed, or ByCategory");

        RuleFor(x => x.VarianceThreshold)
            .GreaterThanOrEqualTo(0)
            .WithMessage("Variance threshold must be greater than or equal to zero");
    }
}

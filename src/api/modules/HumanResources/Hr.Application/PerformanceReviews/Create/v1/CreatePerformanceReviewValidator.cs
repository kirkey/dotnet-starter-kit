namespace FSH.Starter.WebApi.HumanResources.Application.PerformanceReviews.Create.v1;

/// <summary>
/// Validator for CreatePerformanceReviewCommand.
/// </summary>
public class CreatePerformanceReviewValidator : AbstractValidator<CreatePerformanceReviewCommand>
{
    public CreatePerformanceReviewValidator()
    {
        RuleFor(x => x.EmployeeId)
            .NotEmpty().WithMessage("Employee ID is required.");

        RuleFor(x => x.ReviewerId)
            .NotEmpty().WithMessage("Reviewer ID is required.");

        RuleFor(x => x.ReviewPeriodStart)
            .NotEmpty().WithMessage("Review period start date is required.");

        RuleFor(x => x.ReviewPeriodEnd)
            .NotEmpty().WithMessage("Review period end date is required.")
            .GreaterThan(x => x.ReviewPeriodStart).WithMessage("Review period end must be after start date.");

        RuleFor(x => x.ReviewType)
            .NotEmpty().WithMessage("Review type is required.")
            .MaximumLength(64).WithMessage("Review type must not exceed 50 characters.");

        RuleFor(x => x.OverallRating)
            .InclusiveBetween(0, 5).WithMessage("Overall rating must be between 0 and 5.")
            .When(x => x.OverallRating > 0);

        RuleFor(x => x.Strengths)
            .MaximumLength(2048).WithMessage("Strengths must not exceed 2000 characters.")
            .When(x => !string.IsNullOrWhiteSpace(x.Strengths));

        RuleFor(x => x.AreasForImprovement)
            .MaximumLength(2048).WithMessage("Areas for improvement must not exceed 2000 characters.")
            .When(x => !string.IsNullOrWhiteSpace(x.AreasForImprovement));

        RuleFor(x => x.Goals)
            .MaximumLength(2048).WithMessage("Goals must not exceed 2000 characters.")
            .When(x => !string.IsNullOrWhiteSpace(x.Goals));

        RuleFor(x => x.ReviewerComments)
            .MaximumLength(2048).WithMessage("Reviewer comments must not exceed 2000 characters.")
            .When(x => !string.IsNullOrWhiteSpace(x.ReviewerComments));

        RuleFor(x => x.EmployeeComments)
            .MaximumLength(2048).WithMessage("Employee comments must not exceed 2000 characters.")
            .When(x => !string.IsNullOrWhiteSpace(x.EmployeeComments));
    }
}


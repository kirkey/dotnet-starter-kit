namespace FSH.Starter.WebApi.HumanResources.Application.PerformanceReviews.Update.v1;

/// <summary>
/// Validator for UpdatePerformanceReviewCommand.
/// </summary>
public class UpdatePerformanceReviewValidator : AbstractValidator<UpdatePerformanceReviewCommand>
{
    public UpdatePerformanceReviewValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Review ID is required.");

        RuleFor(x => x.OverallRating)
            .InclusiveBetween(1, 5).WithMessage("Overall rating must be between 1 and 5.")
            .When(x => x.OverallRating.HasValue);

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


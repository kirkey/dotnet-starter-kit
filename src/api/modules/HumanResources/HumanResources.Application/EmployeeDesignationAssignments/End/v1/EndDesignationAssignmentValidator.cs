using FluentValidation;

namespace FSH.Starter.WebApi.HumanResources.Application.DesignationAssignments.End.v1;

/// <summary>
/// Validator for EndDesignationAssignmentCommand.
/// </summary>
public class EndDesignationAssignmentValidator : AbstractValidator<EndDesignationAssignmentCommand>
{
    public EndDesignationAssignmentValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("ID is required.");

        RuleFor(x => x.EndDate)
            .NotEmpty().WithMessage("End date is required.")
            .GreaterThanOrEqualTo(DateTime.UtcNow.Date)
            .WithMessage("End date cannot be in the past.");

        RuleFor(x => x.Reason)
            .MaximumLength(500).WithMessage("Reason must not exceed 500 characters.")
            .When(x => !string.IsNullOrWhiteSpace(x.Reason));
    }
}


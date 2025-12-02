namespace FSH.Starter.WebApi.HumanResources.Application.BenefitAllocations.Create.v1;

/// <summary>
/// Validator for CreateBenefitAllocationCommand.
/// </summary>
public class CreateBenefitAllocationValidator : AbstractValidator<CreateBenefitAllocationCommand>
{
    public CreateBenefitAllocationValidator()
    {
        RuleFor(x => x.EnrollmentId)
            .NotEmpty().WithMessage("Enrollment ID is required.");

        RuleFor(x => x.AllocatedAmount)
            .GreaterThan(0).WithMessage("Allocated amount must be greater than zero.");

        RuleFor(x => x.AllocationType)
            .NotEmpty().WithMessage("Allocation type is required.")
            .MaximumLength(64).WithMessage("Allocation type must not exceed 50 characters.");

        RuleFor(x => x.ReferenceNumber)
            .MaximumLength(64).WithMessage("Reference number must not exceed 50 characters.")
            .When(x => !string.IsNullOrWhiteSpace(x.ReferenceNumber));

        RuleFor(x => x.Remarks)
            .MaximumLength(512).WithMessage("Remarks must not exceed 500 characters.")
            .When(x => !string.IsNullOrWhiteSpace(x.Remarks));
    }
}


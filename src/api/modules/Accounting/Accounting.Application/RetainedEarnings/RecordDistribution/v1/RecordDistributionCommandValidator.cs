namespace Accounting.Application.RetainedEarnings.RecordDistribution.v1;

public sealed class RecordDistributionCommandValidator : AbstractValidator<RecordDistributionCommand>
{
    public RecordDistributionCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty().WithMessage("Retained earnings ID is required.");
        RuleFor(x => x.Amount).GreaterThan(0).WithMessage("Distribution amount must be positive.")
            .LessThanOrEqualTo(999999999.99m).WithMessage("Distribution amount must not exceed 999,999,999.99.");
        RuleFor(x => x.DistributionDate).NotEmpty().WithMessage("Distribution date is required.");
        RuleFor(x => x.DistributionType).NotEmpty().WithMessage("Distribution type is required.")
            .MaximumLength(100).WithMessage("Distribution type must not exceed 100 characters.");
    }
}


namespace Accounting.Application.RetainedEarnings.RecordDistribution.v1;

public sealed class RecordDistributionCommandValidator : AbstractValidator<RecordDistributionCommand>
{
    public RecordDistributionCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty().WithMessage("Retained earnings ID is required.");
        RuleFor(x => x.Amount).GreaterThan(0).WithMessage("Distribution amount must be positive.")
            .LessThanOrEqualTo(999999999.99m).WithMessage("Distribution amount must not exceed 999,999,999.99.");
        RuleFor(x => x.DistributionDate).NotEmpty().WithMessage("Distribution date is required.");
        RuleFor(x => x.Description)
            .MaximumLength(500).WithMessage("Description must not exceed 500 characters.");
        RuleFor(x => x.Notes)
            .MaximumLength(2000).WithMessage("Notes must not exceed 2000 characters.");
    }
}


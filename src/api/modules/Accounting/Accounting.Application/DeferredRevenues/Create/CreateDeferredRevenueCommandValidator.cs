namespace Accounting.Application.DeferredRevenues.Create;

public sealed class CreateDeferredRevenueCommandValidator : AbstractValidator<CreateDeferredRevenueCommand>
{
    public CreateDeferredRevenueCommandValidator()
    {
        RuleFor(x => x.DeferredRevenueNumber)
            .NotEmpty().WithMessage("Deferred revenue number is required.")
            .MaximumLength(64).WithMessage("Deferred revenue number must not exceed 50 characters.");

        RuleFor(x => x.RecognitionDate)
            .NotEmpty().WithMessage("Recognition date is required.");

        RuleFor(x => x.Amount)
            .GreaterThan(0).WithMessage("Amount must be positive.")
            .LessThanOrEqualTo(999999999.99m).WithMessage("Amount must not exceed 999,999,999.99.");

        RuleFor(x => x.Description)
            .MaximumLength(512).WithMessage("Description must not exceed 500 characters.")
            .When(x => !string.IsNullOrWhiteSpace(x.Description));
    }
}


namespace Accounting.Application.DeferredRevenues.Update;

public sealed class UpdateDeferredRevenueCommandValidator : AbstractValidator<UpdateDeferredRevenueCommand>
{
    public UpdateDeferredRevenueCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Deferred revenue ID is required.");

        RuleFor(x => x.Amount)
            .GreaterThan(0).WithMessage("Amount must be positive.")
            .LessThanOrEqualTo(999999999.99m).WithMessage("Amount must not exceed 999,999,999.99.")
            .When(x => x.Amount.HasValue);

        RuleFor(x => x.Description)
            .MaximumLength(512).WithMessage("Description must not exceed 500 characters.")
            .When(x => !string.IsNullOrWhiteSpace(x.Description));
    }
}


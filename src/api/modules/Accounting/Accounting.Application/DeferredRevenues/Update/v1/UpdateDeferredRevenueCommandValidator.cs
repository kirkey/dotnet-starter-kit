namespace Accounting.Application.DeferredRevenues.Update.v1;

public sealed class UpdateDeferredRevenueCommandValidator : AbstractValidator<UpdateDeferredRevenueCommand>
{
    public UpdateDeferredRevenueCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty().WithMessage("Deferred revenue ID is required.");
        RuleFor(x => x.Description).MaximumLength(2048).WithMessage("Description must not exceed 2048 characters.")
            .When(x => !string.IsNullOrWhiteSpace(x.Description));
        RuleFor(x => x).Must(x => !string.IsNullOrWhiteSpace(x.Description) || x.RecognitionDate.HasValue)
            .WithMessage("At least one field must be provided for update.");
    }
}


namespace Accounting.Application.DeferredRevenues.Recognize;

public sealed class RecognizeDeferredRevenueCommandValidator : AbstractValidator<RecognizeDeferredRevenueCommand>
{
    public RecognizeDeferredRevenueCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Deferred revenue ID is required.");

        RuleFor(x => x.RecognizedDate)
            .NotEmpty().WithMessage("Recognized date is required.");
    }
}


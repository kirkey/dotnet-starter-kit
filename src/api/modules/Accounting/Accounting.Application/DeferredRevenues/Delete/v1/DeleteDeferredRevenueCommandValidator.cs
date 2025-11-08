namespace Accounting.Application.DeferredRevenues.Delete.v1;

public sealed class DeleteDeferredRevenueCommandValidator : AbstractValidator<DeleteDeferredRevenueCommand>
{
    public DeleteDeferredRevenueCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty().WithMessage("Deferred revenue ID is required.");
    }
}


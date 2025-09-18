using Accounting.Application.DeferredRevenues.Commands;

namespace Accounting.Application.DeferredRevenues.Validators;

public class RecognizeDeferredRevenueCommandValidator : AbstractValidator<RecognizeDeferredRevenueCommand>
{
    public RecognizeDeferredRevenueCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.RecognizedDate).NotEmpty();
    }
}
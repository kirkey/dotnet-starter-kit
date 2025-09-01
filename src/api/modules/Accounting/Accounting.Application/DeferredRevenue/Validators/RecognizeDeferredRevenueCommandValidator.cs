using FluentValidation;
using Accounting.Application.DeferredRevenue.Commands;

namespace Accounting.Application.DeferredRevenue.Validators
{
    public class RecognizeDeferredRevenueCommandValidator : AbstractValidator<RecognizeDeferredRevenueCommand>
    {
        public RecognizeDeferredRevenueCommandValidator()
        {
            RuleFor(x => x.Id).NotEmpty();
            RuleFor(x => x.RecognizedDate).NotEmpty();
        }
    }
}


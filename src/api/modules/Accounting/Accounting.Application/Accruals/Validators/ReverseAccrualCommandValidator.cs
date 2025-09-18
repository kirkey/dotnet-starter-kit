using Accounting.Application.Accruals.Commands;

namespace Accounting.Application.Accruals.Validators;

public class ReverseAccrualCommandValidator : AbstractValidator<ReverseAccrualCommand>
{
    public ReverseAccrualCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.ReversalDate).NotEmpty();
    }
}
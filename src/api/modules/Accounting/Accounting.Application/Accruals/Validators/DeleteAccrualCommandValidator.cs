using Accounting.Application.Accruals.Commands;

namespace Accounting.Application.Accruals.Validators;

public class DeleteAccrualCommandValidator : AbstractValidator<DeleteAccrualCommand>
{
    public DeleteAccrualCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}
using Accounting.Application.Accruals.Delete;

namespace Accounting.Application.Accruals.Validators;

public class DeleteAccrualCommandValidator : AbstractValidator<DeleteAccrualCommand>
{
    public DeleteAccrualCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}
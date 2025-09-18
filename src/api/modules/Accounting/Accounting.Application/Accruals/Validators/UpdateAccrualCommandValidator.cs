using Accounting.Application.Accruals.Commands;

namespace Accounting.Application.Accruals.Validators;

public class UpdateAccrualCommandValidator : AbstractValidator<UpdateAccrualCommand>
{
    public UpdateAccrualCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.AccrualNumber).MaximumLength(50).When(x => !string.IsNullOrEmpty(x.AccrualNumber));
        RuleFor(x => x.AccrualDate).NotEmpty().When(x => x.AccrualDate.HasValue);
        RuleFor(x => x.Amount).GreaterThan(0).When(x => x.Amount.HasValue);
        RuleFor(x => x.Description).MaximumLength(200).When(x => !string.IsNullOrEmpty(x.Description));
    }
}
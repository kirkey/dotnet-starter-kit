using FluentValidation;
using Accounting.Application.Accruals.Commands;

namespace Accounting.Application.Accruals.Validators
{
    public class CreateAccrualCommandValidator : AbstractValidator<CreateAccrualCommand>
    {
        public CreateAccrualCommandValidator()
        {
            RuleFor(x => x.AccrualNumber).NotEmpty().MaximumLength(50);
            RuleFor(x => x.AccrualDate).NotEmpty();
            RuleFor(x => x.Amount).GreaterThan(0);
            RuleFor(x => x.Description).NotEmpty().MaximumLength(200);
        }
    }
}


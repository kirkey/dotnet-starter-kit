using FluentValidation;

namespace FSH.Starter.WebApi.MicroFinance.Application.LoanGuarantors.UpdateAmount.v1;

/// <summary>
/// Validator for UpdateGuaranteedAmountCommand.
/// </summary>
public class UpdateGuaranteedAmountCommandValidator : AbstractValidator<UpdateGuaranteedAmountCommand>
{
    public UpdateGuaranteedAmountCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Guarantor ID is required.");

        RuleFor(x => x.GuaranteedAmount)
            .GreaterThan(0).WithMessage("Guaranteed amount must be greater than zero.");
    }
}

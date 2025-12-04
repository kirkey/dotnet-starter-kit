using FluentValidation;
using FSH.Starter.WebApi.MicroFinance.Domain;

namespace FSH.Starter.WebApi.MicroFinance.Application.FeeCharges.Create.v1;

/// <summary>
/// Validator for CreateFeeChargeCommand.
/// </summary>
public class CreateFeeChargeCommandValidator : AbstractValidator<CreateFeeChargeCommand>
{
    public CreateFeeChargeCommandValidator()
    {
        RuleFor(x => x.FeeDefinitionId)
            .NotEmpty().WithMessage("Fee definition ID is required.");

        RuleFor(x => x.MemberId)
            .NotEmpty().WithMessage("Member ID is required.");

        RuleFor(x => x.Reference)
            .NotEmpty().WithMessage("Reference is required.")
            .MaximumLength(FeeCharge.ReferenceMaxLength);

        RuleFor(x => x.Amount)
            .GreaterThan(0).WithMessage("Amount must be greater than zero.");
    }
}

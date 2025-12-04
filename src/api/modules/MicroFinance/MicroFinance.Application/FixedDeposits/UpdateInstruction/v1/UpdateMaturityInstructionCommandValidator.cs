using FluentValidation;
using FSH.Starter.WebApi.MicroFinance.Domain;

namespace FSH.Starter.WebApi.MicroFinance.Application.FixedDeposits.UpdateInstruction.v1;

/// <summary>
/// Validator for UpdateMaturityInstructionCommand.
/// </summary>
public class UpdateMaturityInstructionCommandValidator : AbstractValidator<UpdateMaturityInstructionCommand>
{
    public UpdateMaturityInstructionCommandValidator()
    {
        RuleFor(x => x.DepositId)
            .NotEmpty().WithMessage("Deposit ID is required.");

        RuleFor(x => x.Instruction)
            .NotEmpty().WithMessage("Maturity instruction is required.")
            .MaximumLength(FixedDeposit.MaturityInstructionMaxLength);
    }
}

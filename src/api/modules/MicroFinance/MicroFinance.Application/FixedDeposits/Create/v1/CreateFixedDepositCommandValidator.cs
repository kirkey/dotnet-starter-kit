using FluentValidation;
using FSH.Starter.WebApi.MicroFinance.Domain;

namespace FSH.Starter.WebApi.MicroFinance.Application.FixedDeposits.Create.v1;

public sealed class CreateFixedDepositCommandValidator : AbstractValidator<CreateFixedDepositCommand>
{
    public CreateFixedDepositCommandValidator()
    {
        RuleFor(x => x.CertificateNumber)
            .NotEmpty()
            .MaximumLength(FixedDeposit.CertificateNumberMaxLength)
            .WithMessage($"Certificate number must not exceed {FixedDeposit.CertificateNumberMaxLength} characters.");

        RuleFor(x => x.MemberId)
            .NotEmpty()
            .WithMessage("Member ID is required.");

        RuleFor(x => x.PrincipalAmount)
            .GreaterThan(0)
            .WithMessage("Principal amount must be greater than 0.");

        RuleFor(x => x.InterestRate)
            .GreaterThanOrEqualTo(0)
            .LessThanOrEqualTo(100)
            .WithMessage("Interest rate must be between 0 and 100.");

        RuleFor(x => x.TermMonths)
            .GreaterThan(0)
            .LessThanOrEqualTo(120)
            .WithMessage("Term must be between 1 and 120 months.");

        RuleFor(x => x.MaturityInstruction)
            .MaximumLength(FixedDeposit.MaturityInstructionMaxLength)
            .When(x => !string.IsNullOrEmpty(x.MaturityInstruction))
            .WithMessage($"Maturity instruction must not exceed {FixedDeposit.MaturityInstructionMaxLength} characters.");

        RuleFor(x => x.Notes)
            .MaximumLength(FixedDeposit.NotesMaxLength)
            .When(x => !string.IsNullOrEmpty(x.Notes))
            .WithMessage($"Notes must not exceed {FixedDeposit.NotesMaxLength} characters.");
    }
}

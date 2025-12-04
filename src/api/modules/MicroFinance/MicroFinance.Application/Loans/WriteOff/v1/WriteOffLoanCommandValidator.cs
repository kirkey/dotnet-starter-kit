using FluentValidation;

namespace FSH.Starter.WebApi.MicroFinance.Application.Loans.WriteOff.v1;

public sealed class WriteOffLoanCommandValidator : AbstractValidator<WriteOffLoanCommand>
{
    public WriteOffLoanCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Loan ID is required.");

        RuleFor(x => x.WriteOffReason)
            .NotEmpty()
            .MaximumLength(512)
            .WithMessage("Write-off reason is required and must not exceed 512 characters.");
    }
}

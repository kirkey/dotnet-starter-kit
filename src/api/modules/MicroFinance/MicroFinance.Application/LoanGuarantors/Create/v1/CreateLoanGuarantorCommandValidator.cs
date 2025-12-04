using FluentValidation;
using FSH.Starter.WebApi.MicroFinance.Domain;

namespace FSH.Starter.WebApi.MicroFinance.Application.LoanGuarantors.Create.v1;

public sealed class CreateLoanGuarantorCommandValidator : AbstractValidator<CreateLoanGuarantorCommand>
{
    public CreateLoanGuarantorCommandValidator()
    {
        RuleFor(x => x.LoanId)
            .NotEmpty()
            .WithMessage("Loan ID is required.");

        RuleFor(x => x.GuarantorMemberId)
            .NotEmpty()
            .WithMessage("Guarantor member ID is required.");

        RuleFor(x => x.GuaranteedAmount)
            .GreaterThan(0)
            .WithMessage("Guaranteed amount must be greater than 0.");

        RuleFor(x => x.Relationship)
            .MaximumLength(LoanGuarantor.RelationshipMaxLength)
            .When(x => !string.IsNullOrWhiteSpace(x.Relationship))
            .WithMessage($"Relationship must not exceed {LoanGuarantor.RelationshipMaxLength} characters.");

        RuleFor(x => x.ExpiryDate)
            .GreaterThan(x => x.GuaranteeDate ?? DateOnly.FromDateTime(DateTime.UtcNow))
            .When(x => x.ExpiryDate.HasValue)
            .WithMessage("Expiry date must be after guarantee date.");

        RuleFor(x => x.Notes)
            .MaximumLength(LoanGuarantor.NotesMaxLength)
            .When(x => !string.IsNullOrWhiteSpace(x.Notes))
            .WithMessage($"Notes must not exceed {LoanGuarantor.NotesMaxLength} characters.");
    }
}

using Accounting.Application.Payees.Queries;

namespace Accounting.Application.Payees.Create.v1;

/// <summary>
/// Validator for PayeeCreateCommand implementing stricter and tighter validations.
/// Ensures data integrity and business rule compliance for payee creation.
/// </summary>
public class PayeeCreateCommandValidator : AbstractValidator<PayeeCreateCommand>
{
    /// <summary>
    /// Initializes a new instance of the PayeeCreateCommandValidator class.
    /// </summary>
    /// <param name="repository">Repository for validating uniqueness constraints.</param>
    public PayeeCreateCommandValidator(IReadRepository<Payee> repository)
    {
        ArgumentNullException.ThrowIfNull(repository);

        RuleFor(p => p.PayeeCode)
            .NotEmpty()
            .WithMessage("Payee Code is required and cannot be empty.")
            .Length(3, 32)
            .WithMessage("Payee Code must be between 3 and 32 characters.")
            .Matches(@"^[A-Z0-9\-_]+$")
            .WithMessage("Payee Code must contain only uppercase letters, numbers, hyphens, and underscores.")
            .MustAsync(async (code, cancellation) =>
                (await repository.FirstOrDefaultAsync(new PayeeByCodeSpec(code), cancellation)) == null)
            .WithMessage("Payee Code '{PropertyValue}' already exists.");

        RuleFor(p => p.Name)
            .NotEmpty()
            .WithMessage("Payee Name is required and cannot be empty.")
            .Length(2, 256)
            .WithMessage("Payee Name must be between 2 and 256 characters.")
            .Matches(@"^[a-zA-Z0-9\s\.\-&'(),]+$")
            .WithMessage("Payee Name contains invalid characters.")
            .MustAsync(async (name, cancellation) =>
                (await repository.FirstOrDefaultAsync(new PayeeByNameSpec(name), cancellation)) == null)
            .WithMessage("Payee Name '{PropertyValue}' already exists.");

        RuleFor(p => p.Address)
            .MaximumLength(500)
            .WithMessage("Address cannot exceed 500 characters.")
            .Matches(@"^[a-zA-Z0-9\s\.\-#,]+$")
            .WithMessage("Address contains invalid characters.")
            .When(p => !string.IsNullOrEmpty(p.Address));

        RuleFor(p => p.ExpenseAccountCode)
            .Length(4, 20)
            .WithMessage("Expense Account Code must be between 4 and 20 characters.")
            .Matches(@"^[0-9A-Z\-]+$")
            .WithMessage("Expense Account Code must contain only numbers, uppercase letters, and hyphens.")
            .When(p => !string.IsNullOrEmpty(p.ExpenseAccountCode));

        RuleFor(p => p.Tin)
            .Length(9, 15)
            .WithMessage("TIN must be between 9 and 15 characters.")
            .Matches(@"^[0-9\-]+$")
            .WithMessage("TIN must contain only numbers and hyphens (e.g., 12-3456789 or 123456789).")
            .When(p => !string.IsNullOrEmpty(p.Tin));

        RuleFor(p => p.Description)
            .MaximumLength(1000)
            .WithMessage("Description cannot exceed 1000 characters.")
            .When(p => !string.IsNullOrEmpty(p.Description));

        RuleFor(p => p.Notes)
            .MaximumLength(2000)
            .WithMessage("Notes cannot exceed 2000 characters.")
            .When(p => !string.IsNullOrEmpty(p.Notes));

        RuleFor(p => p.ImageUrl)
            .MaximumLength(500)
            .When(p => !string.IsNullOrEmpty(p.ImageUrl));
    }
}

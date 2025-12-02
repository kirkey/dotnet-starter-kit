namespace Accounting.Application.GeneralLedgers.Update.v1;

/// <summary>
/// Validator for GeneralLedgerUpdateCommand.
/// </summary>
public sealed class GeneralLedgerUpdateCommandValidator : AbstractValidator<GeneralLedgerUpdateCommand>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="GeneralLedgerUpdateCommandValidator"/> class.
    /// </summary>
    public GeneralLedgerUpdateCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("General ledger entry ID is required.");

        RuleFor(x => x.Debit)
            .GreaterThanOrEqualTo(0)
            .WithMessage("Debit amount cannot be negative.")
            .LessThanOrEqualTo(999999999.99m)
            .WithMessage("Debit amount must not exceed 999,999,999.99.")
            .When(x => x.Debit.HasValue);

        RuleFor(x => x.Credit)
            .GreaterThanOrEqualTo(0)
            .WithMessage("Credit amount cannot be negative.")
            .LessThanOrEqualTo(999999999.99m)
            .WithMessage("Credit amount must not exceed 999,999,999.99.")
            .When(x => x.Credit.HasValue);

        RuleFor(x => x.Memo)
            .MaximumLength(512)
            .WithMessage("Memo must not exceed 500 characters.")
            .When(x => !string.IsNullOrWhiteSpace(x.Memo));

        RuleFor(x => x.UsoaClass)
            .MaximumLength(128)
            .WithMessage("USOA class must not exceed 100 characters.")
            .Must(BeValidUsoaClass)
            .WithMessage("Invalid USOA class. Valid values: Generation, Transmission, Distribution, Customer Service, Sales, Administrative, General, Maintenance.")
            .When(x => !string.IsNullOrWhiteSpace(x.UsoaClass));

        RuleFor(x => x.ReferenceNumber)
            .MaximumLength(128)
            .WithMessage("Reference number must not exceed 100 characters.")
            .When(x => !string.IsNullOrWhiteSpace(x.ReferenceNumber));

        RuleFor(x => x.Description)
            .MaximumLength(512)
            .WithMessage("Description must not exceed 500 characters.")
            .When(x => !string.IsNullOrWhiteSpace(x.Description));

        RuleFor(x => x.Notes)
            .MaximumLength(2048)
            .WithMessage("Notes must not exceed 2000 characters.")
            .When(x => !string.IsNullOrWhiteSpace(x.Notes));

        RuleFor(x => x)
            .Must(x => x.Debit.HasValue || x.Credit.HasValue || !string.IsNullOrWhiteSpace(x.Memo) ||
                      !string.IsNullOrWhiteSpace(x.UsoaClass) || !string.IsNullOrWhiteSpace(x.ReferenceNumber) ||
                      !string.IsNullOrWhiteSpace(x.Description) || !string.IsNullOrWhiteSpace(x.Notes))
            .WithMessage("At least one field must be provided for update.");
    }

    private bool BeValidUsoaClass(string? usoaClass)
    {
        if (string.IsNullOrWhiteSpace(usoaClass))
            return true;

        var validClasses = new[] { "Generation", "Transmission", "Distribution", "Customer Service",
            "Sales", "Administrative", "General", "Maintenance" };
        return validClasses.Contains(usoaClass.Trim(), StringComparer.OrdinalIgnoreCase);
    }
}


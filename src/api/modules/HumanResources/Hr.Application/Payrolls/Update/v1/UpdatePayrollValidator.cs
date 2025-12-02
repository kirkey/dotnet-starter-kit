namespace FSH.Starter.WebApi.HumanResources.Application.Payrolls.Update.v1;

/// <summary>
/// Validator for updating a payroll.
/// </summary>
public class UpdatePayrollValidator : AbstractValidator<UpdatePayrollCommand>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="UpdatePayrollValidator"/> class.
    /// </summary>
    public UpdatePayrollValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Payroll ID is required");

        RuleFor(x => x.Status)
            .Must(BeValidStatus)
            .When(x => !string.IsNullOrWhiteSpace(x.Status))
            .WithMessage("Status must be Processing, Processed, Posted, or Paid");

        RuleFor(x => x.JournalEntryId)
            .NotEmpty()
            .WithMessage("Journal entry ID is required when posting")
            .When(x => !string.IsNullOrWhiteSpace(x.Status) && x.Status.Equals("posted", StringComparison.OrdinalIgnoreCase));

        RuleFor(x => x.JournalEntryId)
            .MaximumLength(64)
            .WithMessage("Journal entry ID cannot exceed 50 characters")
            .When(x => !string.IsNullOrWhiteSpace(x.JournalEntryId));

        RuleFor(x => x.Notes)
            .MaximumLength(512)
            .WithMessage("Notes cannot exceed 500 characters")
            .When(x => !string.IsNullOrWhiteSpace(x.Notes));
    }

    private static bool BeValidStatus(string? status)
    {
        if (string.IsNullOrWhiteSpace(status))
            return true;

        var validStatuses = new[] { "Processing", "Processed", "Posted", "Paid" };
        return validStatuses.Contains(status);
    }
}


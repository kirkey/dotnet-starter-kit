namespace Accounting.Application.BankReconciliations.Search.v1;

/// <summary>
/// Validator for SearchBankReconciliationsCommand.
/// Validates search filters and pagination parameters.
/// </summary>
public sealed class SearchBankReconciliationsCommandValidator : AbstractValidator<SearchBankReconciliationsCommand>
{
    public SearchBankReconciliationsCommandValidator()
    {
        RuleFor(x => x.PageNumber)
            .GreaterThanOrEqualTo(1)
            .WithMessage("Page number must be greater than or equal to 1.");

        RuleFor(x => x.PageSize)
            .GreaterThanOrEqualTo(1)
            .WithMessage("Page size must be greater than or equal to 1.")
            .LessThanOrEqualTo(100)
            .WithMessage("Page size cannot exceed 100.");

        RuleFor(x => x.FromDate)
            .LessThanOrEqualTo(x => x.ToDate)
            .WithMessage("From date must be less than or equal to to date.")
            .When(x => x.FromDate.HasValue && x.ToDate.HasValue);

        RuleFor(x => x.FromDate)
            .LessThanOrEqualTo(DateTime.UtcNow.Date.AddDays(1))
            .WithMessage("From date cannot be in the future.")
            .When(x => x.FromDate.HasValue);

        RuleFor(x => x.ToDate)
            .LessThanOrEqualTo(DateTime.UtcNow.Date.AddDays(1))
            .WithMessage("To date cannot be in the future.")
            .When(x => x.ToDate.HasValue);

        RuleFor(x => x.Status)
            .Must(status => string.IsNullOrWhiteSpace(status) || IsValidStatus(status))
            .WithMessage("Status must be one of: Pending, InProgress, Completed, Approved.")
            .When(x => !string.IsNullOrWhiteSpace(x.Status));
    }

    /// <summary>
    /// Validates if the status is one of the allowed reconciliation statuses.
    /// </summary>
    private static bool IsValidStatus(string status)
    {
        var validStatuses = new[] { "Pending", "InProgress", "Completed", "Approved" };
        return validStatuses.Contains(status, StringComparer.OrdinalIgnoreCase);
    }
}


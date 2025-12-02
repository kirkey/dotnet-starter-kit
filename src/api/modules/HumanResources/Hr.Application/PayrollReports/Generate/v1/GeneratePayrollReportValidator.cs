namespace FSH.Starter.WebApi.HumanResources.Application.PayrollReports.Generate.v1;

/// <summary>
/// Validator for GeneratePayrollReportCommand.
/// </summary>
public sealed class GeneratePayrollReportValidator : AbstractValidator<GeneratePayrollReportCommand>
{
    /// <summary>
    /// Initializes the validator with validation rules.
    /// </summary>
    public GeneratePayrollReportValidator()
    {
        RuleFor(x => x.ReportType)
            .NotEmpty().WithMessage("Report type is required")
            .Must(x => IsValidReportType(x)).WithMessage("Invalid report type");

        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("Report title is required")
            .MaximumLength(256).WithMessage("Report title cannot exceed 200 characters");

        RuleFor(x => x.FromDate)
            .NotNull().WithMessage("From date is required");

        RuleFor(x => x.ToDate)
            .NotNull().WithMessage("To date is required")
            .GreaterThanOrEqualTo(x => x.FromDate).WithMessage("To date must be after from date");

        RuleFor(x => x.Notes)
            .MaximumLength(1024).WithMessage("Notes cannot exceed 1000 characters");
    }

    /// <summary>
    /// Validates that the report type is one of the supported values.
    /// </summary>
    private static bool IsValidReportType(string reportType)
    {
        var validTypes = new[]
        {
            "Summary",
            "Detailed",
            "Departmental",
            "ByEmployee",
            "TaxReport",
            "DeductionReport",
            "BankTransfer"
        };
        return validTypes.Contains(reportType);
    }
}


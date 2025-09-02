using FluentValidation;

namespace Accounting.Application.RegulatoryReports.Create.v1;

public class RegulatoryReportCreateRequestValidator : AbstractValidator<RegulatoryReportCreateRequest>
{
    public RegulatoryReportCreateRequestValidator()
    {
        RuleFor(x => x.ReportName)
            .NotEmpty()
            .MaximumLength(200)
            .WithMessage("Report name is required and must not exceed 200 characters");

        RuleFor(x => x.ReportType)
            .NotEmpty()
            .Must(BeValidReportType)
            .WithMessage("Report type must be one of: FERC Form 1, FERC Form 2, FERC Form 6, EIA Form 861, State Commission");

        RuleFor(x => x.ReportingPeriod)
            .NotEmpty()
            .Must(BeValidReportingPeriod)
            .WithMessage("Reporting period must be one of: Annual, Monthly, Quarterly");

        RuleFor(x => x.PeriodStartDate)
            .NotEmpty()
            .WithMessage("Period start date is required");

        RuleFor(x => x.PeriodEndDate)
            .NotEmpty()
            .GreaterThan(x => x.PeriodStartDate)
            .WithMessage("Period end date must be after the start date");

        RuleFor(x => x.DueDate)
            .NotEmpty()
            .GreaterThanOrEqualTo(x => x.PeriodEndDate)
            .WithMessage("Due date must be on or after the period end date");

        RuleFor(x => x.RegulatoryBody)
            .MaximumLength(100)
            .When(x => !string.IsNullOrEmpty(x.RegulatoryBody))
            .WithMessage("Regulatory body must not exceed 100 characters");
    }

    private static bool BeValidReportType(string reportType)
    {
        var validTypes = new[] { "FERC Form 1", "FERC Form 2", "FERC Form 6", "EIA Form 861", "State Commission" };
        return validTypes.Contains(reportType);
    }

    private static bool BeValidReportingPeriod(string reportingPeriod)
    {
        var validPeriods = new[] { "Annual", "Monthly", "Quarterly" };
        return validPeriods.Contains(reportingPeriod);
    }
}

namespace FSH.Starter.WebApi.HumanResources.Application.LeaveTypes.Create.v1;

/// <summary>
/// Validator for CreateLeaveTypeCommand with Philippines Labor Code compliance rules.
/// </summary>
public class CreateLeaveTypeValidator : AbstractValidator<CreateLeaveTypeCommand>
{
    public CreateLeaveTypeValidator()
    {
        // Basic Required Fields
        RuleFor(x => x.LeaveName)
            .NotEmpty().WithMessage("Leave name is required")
            .MaximumLength(128).WithMessage("Leave name cannot exceed 100 characters");

        RuleFor(x => x.AnnualAllowance)
            .GreaterThan(0).WithMessage("Annual allowance must be greater than 0")
            .LessThanOrEqualTo(365).WithMessage("Annual allowance cannot exceed 365 days");

        RuleFor(x => x.AccrualFrequency)
            .Must(BeValidFrequency).WithMessage("Accrual frequency must be Monthly, Quarterly, or Annual");

        RuleFor(x => x.MaxCarryoverDays)
            .GreaterThanOrEqualTo(0).WithMessage("Max carryover days cannot be negative");

        RuleFor(x => x.MinimumNoticeDay)
            .GreaterThan(0).WithMessage("Minimum notice days must be greater than 0")
            .When(x => x.MinimumNoticeDay.HasValue);

        RuleFor(x => x.Description)
            .MaximumLength(512).WithMessage("Description cannot exceed 500 characters")
            .When(x => !string.IsNullOrWhiteSpace(x.Description));

        // Philippines-Specific: Leave Code Validation
        RuleFor(x => x.LeaveCode)
            .Must(BeValidLeaveCode)
            .WithMessage("Leave code must be: VacationLeave, SickLeave, MaternityLeave, PaternityLeave, SpecialLeave, or SoloParentLeave")
            .When(x => !string.IsNullOrWhiteSpace(x.LeaveCode));

        // Philippines-Specific: Gender Validation
        RuleFor(x => x.ApplicableGender)
            .Must(g => new[] { "Both", "Male", "Female" }.Contains(g))
            .WithMessage("Applicable gender must be: Both, Male, or Female");

        // Philippines-Specific: Minimum Service Days Validation
        RuleFor(x => x.MinimumServiceDays)
            .GreaterThanOrEqualTo(0).WithMessage("Minimum service days cannot be negative")
            .LessThanOrEqualTo(365).WithMessage("Minimum service days cannot exceed 365 days");

        // Philippines-Specific: Medical Certificate Validation
        RuleFor(x => x.MedicalCertificateAfterDays)
            .GreaterThan(0).WithMessage("Medical certificate after days must be greater than 0")
            .When(x => x.RequiresMedicalCertification);

        // Philippines-Specific: Labor Code Compliance Rules
        RuleFor(x => x)
            .Must(ValidateVacationLeaveRules)
            .WithMessage("Vacation Leave must be cumulative and convertible to cash per Labor Code Article 95")
            .When(x => x.LeaveCode == "VacationLeave");

        RuleFor(x => x)
            .Must(ValidateSickLeaveRules)
            .WithMessage("Sick Leave must be non-cumulative and NOT convertible to cash per Labor Code Article 96")
            .When(x => x.LeaveCode == "SickLeave");

        RuleFor(x => x)
            .Must(ValidateMaternityLeaveRules)
            .WithMessage("Maternity Leave must be for Female only and at least 105 days per RA 11210")
            .When(x => x.LeaveCode == "MaternityLeave");

        RuleFor(x => x)
            .Must(ValidatePaternityLeaveRules)
            .WithMessage("Paternity Leave must be for Male only and at least 7 days per Labor Code Article 98")
            .When(x => x.LeaveCode == "PaternityLeave");
    }

    private static bool BeValidFrequency(string? frequency)
    {
        if (string.IsNullOrWhiteSpace(frequency))
            return false;

        var validFrequencies = new[] { "Monthly", "Quarterly", "Annual", "AsNeeded" };
        return validFrequencies.Contains(frequency);
    }

    private static bool BeValidLeaveCode(string? leaveCode)
    {
        if (string.IsNullOrWhiteSpace(leaveCode))
            return true; // Optional field

        var validCodes = new[]
        {
            "VacationLeave",
            "SickLeave",
            "MaternityLeave",
            "PaternityLeave",
            "SpecialLeave",
            "SoloParentLeave",
            "ReproductiveHealthLeave"
        };

        return validCodes.Contains(leaveCode);
    }

    // Philippines-Specific: Vacation Leave must be cumulative and convertible per Art 95
    private static bool ValidateVacationLeaveRules(CreateLeaveTypeCommand command)
    {
        if (command.LeaveCode != "VacationLeave")
            return true;

        return command is { IsCumulative: true, IsConvertibleToCash: true, AnnualAllowance: >= 5 };
    }

    // Philippines-Specific: Sick Leave must be non-cumulative and NOT convertible per Art 96
    private static bool ValidateSickLeaveRules(CreateLeaveTypeCommand command)
    {
        if (command.LeaveCode != "SickLeave")
            return true;

        return command is { IsCumulative: false, IsConvertibleToCash: false, AnnualAllowance: >= 5 };
    }

    // Philippines-Specific: Maternity Leave must be Female only, 105 days minimum per RA 11210
    private static bool ValidateMaternityLeaveRules(CreateLeaveTypeCommand command)
    {
        if (command.LeaveCode != "MaternityLeave")
            return true;

        return command is { ApplicableGender: "Female", AnnualAllowance: >= 105 };
    }

    // Philippines-Specific: Paternity Leave must be Male only, 7 days minimum per Art 98
    private static bool ValidatePaternityLeaveRules(CreateLeaveTypeCommand command)
    {
        if (command.LeaveCode != "PaternityLeave")
            return true;

        return command is { ApplicableGender: "Male", AnnualAllowance: >= 7 };
    }
}


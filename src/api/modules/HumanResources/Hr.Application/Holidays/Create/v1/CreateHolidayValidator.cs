namespace FSH.Starter.WebApi.HumanResources.Application.Holidays.Create.v1;

/// <summary>
/// Validator for CreateHolidayCommand with Philippines Labor Code compliance rules.
/// </summary>
public class CreateHolidayValidator : AbstractValidator<CreateHolidayCommand>
{
    public CreateHolidayValidator()
    {
        // Basic Required Fields
        RuleFor(x => x.HolidayName)
            .NotEmpty().WithMessage("Holiday name is required.")
            .MaximumLength(100).WithMessage("Holiday name must not exceed 100 characters.");

        RuleFor(x => x.HolidayDate)
            .NotEmpty().WithMessage("Holiday date is required.")
            .GreaterThanOrEqualTo(DateTime.Today.AddYears(-1)).WithMessage("Holiday date cannot be more than 1 year in the past.")
            .LessThanOrEqualTo(DateTime.Today.AddYears(5)).WithMessage("Holiday date cannot be more than 5 years in the future.");

        RuleFor(x => x.Description)
            .MaximumLength(500).WithMessage("Description must not exceed 500 characters.")
            .When(x => !string.IsNullOrWhiteSpace(x.Description));

        // Philippines-Specific: Holiday Type Validation
        RuleFor(x => x.Type)
            .Must(BeValidHolidayType)
            .WithMessage("Holiday type must be: RegularPublicHoliday or SpecialNonWorkingDay");

        // Philippines-Specific: Pay Rate Multiplier Validation
        RuleFor(x => x.PayRateMultiplier)
            .GreaterThanOrEqualTo(0).WithMessage("Pay rate multiplier cannot be negative.")
            .LessThanOrEqualTo(3.0m).WithMessage("Pay rate multiplier cannot exceed 300% (3.0).");

        // Philippines-Specific: Standard Pay Rates Validation
        RuleFor(x => x)
            .Must(ValidateStandardPayRates)
            .WithMessage("Regular Public Holiday should have 1.0 (100%) pay rate, Special Non-Working Day should have 0.3 (30%) pay rate")
            .When(x => x.Type == "RegularPublicHoliday" || x.Type == "SpecialNonWorkingDay");

        // Philippines-Specific: Moveable Rule Validation
        RuleFor(x => x.MoveableRule)
            .NotEmpty().WithMessage("Moveable rule is required when holiday is moveable.")
            .MaximumLength(200).WithMessage("Moveable rule must not exceed 200 characters.")
            .When(x => x.IsMoveable);

        // Philippines-Specific: Regional Applicability Validation
        RuleFor(x => x.ApplicableRegions)
            .NotEmpty().WithMessage("Applicable regions are required when holiday is not nationwide.")
            .MaximumLength(500).WithMessage("Applicable regions must not exceed 500 characters.")
            .When(x => !x.IsNationwide);
    }

    private static bool BeValidHolidayType(string type)
    {
        var validTypes = new[] { "RegularPublicHoliday", "SpecialNonWorkingDay" };
        return validTypes.Contains(type);
    }

    private static bool ValidateStandardPayRates(CreateHolidayCommand command)
    {
        // Regular Public Holiday: Standard 100% (1.0) premium
        if (command.Type == "RegularPublicHoliday" && command.PayRateMultiplier != 1.0m)
            return false;

        // Special Non-Working Day: Standard 30% (0.3) premium
        if (command.Type == "SpecialNonWorkingDay" && command.PayRateMultiplier != 0.3m)
            return false;

        return true;
    }
}


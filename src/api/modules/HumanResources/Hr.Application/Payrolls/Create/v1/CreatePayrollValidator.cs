namespace FSH.Starter.WebApi.HumanResources.Application.Payrolls.Create.v1;

/// <summary>
/// Validator for creating a payroll.
/// </summary>
public class CreatePayrollValidator : AbstractValidator<CreatePayrollCommand>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="CreatePayrollValidator"/> class.
    /// </summary>
    public CreatePayrollValidator()
    {
        RuleFor(x => x.StartDate)
            .NotEmpty()
            .WithMessage("Start date is required");

        RuleFor(x => x.EndDate)
            .NotEmpty()
            .WithMessage("End date is required")
            .GreaterThan(x => x.StartDate)
            .WithMessage("End date must be after start date");

        RuleFor(x => x.PayFrequency)
            .NotEmpty()
            .WithMessage("Pay frequency is required")
            .Must(BeValidFrequency)
            .WithMessage("Pay frequency must be Weekly, BiWeekly, SemiMonthly, or Monthly");

        RuleFor(x => x.Notes)
            .MaximumLength(512)
            .WithMessage("Notes cannot exceed 500 characters")
            .When(x => !string.IsNullOrWhiteSpace(x.Notes));
    }

    private static bool BeValidFrequency(string? frequency)
    {
        if (string.IsNullOrWhiteSpace(frequency))
            return false;

        var validFrequencies = new[] { "Weekly", "BiWeekly", "SemiMonthly", "Monthly" };
        return validFrequencies.Contains(frequency);
    }
}


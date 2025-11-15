namespace FSH.Starter.WebApi.HumanResources.Application.PayComponentRates.Update.v1;

/// <summary>
/// Validator for UpdatePayComponentRateCommand.
/// </summary>
public sealed class UpdatePayComponentRateValidator : AbstractValidator<UpdatePayComponentRateCommand>
{
    public UpdatePayComponentRateValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Pay component rate ID is required.");

        RuleFor(x => x.EmployeeRate)
            .GreaterThanOrEqualTo(0m).WithMessage("Employee rate must be non-negative.")
            .LessThanOrEqualTo(1m).WithMessage("Employee rate cannot exceed 100%.")
            .When(x => x.EmployeeRate.HasValue);

        RuleFor(x => x.EmployerRate)
            .GreaterThanOrEqualTo(0m).WithMessage("Employer rate must be non-negative.")
            .LessThanOrEqualTo(1m).WithMessage("Employer rate cannot exceed 100%.")
            .When(x => x.EmployerRate.HasValue);

        RuleFor(x => x.AdditionalEmployerRate)
            .GreaterThanOrEqualTo(0m).WithMessage("Additional employer rate must be non-negative.")
            .LessThanOrEqualTo(1m).WithMessage("Additional employer rate cannot exceed 100%.")
            .When(x => x.AdditionalEmployerRate.HasValue);

        RuleFor(x => x.TaxRate)
            .GreaterThanOrEqualTo(0m).WithMessage("Tax rate must be non-negative.")
            .LessThanOrEqualTo(1m).WithMessage("Tax rate cannot exceed 100%.")
            .When(x => x.TaxRate.HasValue);

        RuleFor(x => x.EmployeeAmount)
            .GreaterThanOrEqualTo(0m).WithMessage("Employee amount must be non-negative.")
            .When(x => x.EmployeeAmount.HasValue);

        RuleFor(x => x.EmployerAmount)
            .GreaterThanOrEqualTo(0m).WithMessage("Employer amount must be non-negative.")
            .When(x => x.EmployerAmount.HasValue);

        RuleFor(x => x.BaseAmount)
            .GreaterThanOrEqualTo(0m).WithMessage("Base amount must be non-negative.")
            .When(x => x.BaseAmount.HasValue);

        RuleFor(x => x.ExcessRate)
            .GreaterThanOrEqualTo(0m).WithMessage("Excess rate must be non-negative.")
            .LessThanOrEqualTo(1m).WithMessage("Excess rate cannot exceed 100%.")
            .When(x => x.ExcessRate.HasValue);
    }
}



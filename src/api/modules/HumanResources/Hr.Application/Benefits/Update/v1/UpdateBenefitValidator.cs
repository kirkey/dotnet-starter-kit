namespace FSH.Starter.WebApi.HumanResources.Application.Benefits.Update.v1;

/// <summary>
/// Validator for UpdateBenefitCommand.
/// </summary>
public sealed class UpdateBenefitValidator : AbstractValidator<UpdateBenefitCommand>
{
    public UpdateBenefitValidator()
    {
        RuleFor(x => x.EmployeeContribution)
            .GreaterThanOrEqualTo(0)
            .When(x => x.EmployeeContribution.HasValue);

        RuleFor(x => x.EmployerContribution)
            .GreaterThanOrEqualTo(0)
            .When(x => x.EmployerContribution.HasValue);

        RuleFor(x => x.CoverageAmount)
            .GreaterThan(0)
            .When(x => x.CoverageAmount.HasValue);

        RuleFor(x => x.CoverageType)
            .MaximumLength(64)
            .When(x => !string.IsNullOrWhiteSpace(x.CoverageType));

        RuleFor(x => x.ProviderName)
            .MaximumLength(128)
            .When(x => !string.IsNullOrWhiteSpace(x.ProviderName));

        RuleFor(x => x.Description)
            .MaximumLength(512)
            .When(x => !string.IsNullOrWhiteSpace(x.Description));
    }
}


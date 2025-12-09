using FluentValidation;
using FSH.Starter.WebApi.MicroFinance.Domain;

namespace FSH.Starter.WebApi.MicroFinance.Application.RiskCategories.Update.v1;

/// <summary>
/// Validator for UpdateRiskCategoryCommand.
/// </summary>
public class UpdateRiskCategoryCommandValidator : AbstractValidator<UpdateRiskCategoryCommand>
{
    public UpdateRiskCategoryCommandValidator()
    {
        RuleFor(c => c.Id)
            .NotEmpty()
            .WithMessage("Risk category ID is required.");

        RuleFor(c => c.Name)
            .MaximumLength(RiskCategory.MaxLengths.Name)
            .When(c => !string.IsNullOrWhiteSpace(c.Name));

        RuleFor(c => c.Description)
            .MaximumLength(RiskCategory.MaxLengths.Description)
            .When(c => !string.IsNullOrWhiteSpace(c.Description));

        RuleFor(c => c.DefaultSeverity)
            .Must(s => s == RiskCategory.SeverityLow ||
                       s == RiskCategory.SeverityMedium ||
                       s == RiskCategory.SeverityHigh ||
                       s == RiskCategory.SeverityCritical)
            .When(c => !string.IsNullOrWhiteSpace(c.DefaultSeverity))
            .WithMessage("Invalid severity level. Must be Low, Medium, High, or Critical.");

        RuleFor(c => c.WeightFactor)
            .GreaterThan(0)
            .When(c => c.WeightFactor.HasValue)
            .WithMessage("Weight factor must be greater than 0.");

        RuleFor(c => c.AlertThreshold)
            .GreaterThanOrEqualTo(0)
            .When(c => c.AlertThreshold.HasValue)
            .WithMessage("Alert threshold must be non-negative.");

        RuleFor(c => c.EscalationHours)
            .GreaterThan(0)
            .When(c => c.EscalationHours.HasValue)
            .WithMessage("Escalation hours must be greater than 0.");

        RuleFor(c => c.DisplayOrder)
            .GreaterThanOrEqualTo(0)
            .When(c => c.DisplayOrder.HasValue)
            .WithMessage("Display order must be non-negative.");

        RuleFor(c => c.Notes)
            .MaximumLength(RiskCategory.MaxLengths.Notes)
            .When(c => !string.IsNullOrWhiteSpace(c.Notes));
    }
}

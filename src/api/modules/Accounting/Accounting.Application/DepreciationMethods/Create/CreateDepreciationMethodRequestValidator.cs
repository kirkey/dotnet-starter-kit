namespace Accounting.Application.DepreciationMethods.Create;

public class CreateDepreciationMethodRequestValidator : AbstractValidator<CreateDepreciationMethodRequest>
{
    public CreateDepreciationMethodRequestValidator()
    {
        RuleFor(x => x.MethodCode)
            .NotEmpty()
            .MaximumLength(16);

        RuleFor(x => x.MethodName)
            .NotEmpty()
            .MaximumLength(256);

        RuleFor(x => x.CalculationFormula)
            .MaximumLength(500)
            .When(x => !string.IsNullOrEmpty(x.CalculationFormula));

        RuleFor(x => x.Description)
            .MaximumLength(1000)
            .When(x => !string.IsNullOrEmpty(x.Description));

        RuleFor(x => x.Notes)
            .MaximumLength(1000)
            .When(x => !string.IsNullOrEmpty(x.Notes));
    }
}

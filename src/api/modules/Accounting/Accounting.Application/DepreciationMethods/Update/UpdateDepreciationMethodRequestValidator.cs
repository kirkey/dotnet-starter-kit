namespace Accounting.Application.DepreciationMethods.Update;

public class UpdateDepreciationMethodRequestValidator : AbstractValidator<UpdateDepreciationMethodRequest>
{
    public UpdateDepreciationMethodRequestValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty();

        RuleFor(x => x.MethodCode)
            .MaximumLength(16)
            .When(x => !string.IsNullOrEmpty(x.MethodCode));

        RuleFor(x => x.MethodName)
            .MaximumLength(256)
            .When(x => !string.IsNullOrEmpty(x.MethodName));

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

namespace Accounting.Application.DepreciationMethods.Activate.v1;

public sealed class ActivateDepreciationMethodCommandValidator : AbstractValidator<ActivateDepreciationMethodCommand>
{
    public ActivateDepreciationMethodCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty().WithMessage("Depreciation method ID is required.");
    }
}


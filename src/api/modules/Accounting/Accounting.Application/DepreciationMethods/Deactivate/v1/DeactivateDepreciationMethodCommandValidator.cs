namespace Accounting.Application.DepreciationMethods.Deactivate.v1;

public sealed class DeactivateDepreciationMethodCommandValidator : AbstractValidator<DeactivateDepreciationMethodCommand>
{
    public DeactivateDepreciationMethodCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty().WithMessage("Depreciation method ID is required.");
    }
}


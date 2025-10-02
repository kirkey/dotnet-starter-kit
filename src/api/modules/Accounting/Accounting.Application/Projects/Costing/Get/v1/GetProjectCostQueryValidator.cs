namespace Accounting.Application.Projects.Costing.Get.v1;

/// <summary>
/// Validator for <see cref="GetProjectCostQuery"/> to ensure a valid identifier is provided.
/// </summary>
public sealed class GetProjectCostQueryValidator : AbstractValidator<GetProjectCostQuery>
{
    public GetProjectCostQueryValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .Must(id => id != DefaultIdType.Empty)
            .WithMessage("Id must be a non-empty GUID.");
    }
}


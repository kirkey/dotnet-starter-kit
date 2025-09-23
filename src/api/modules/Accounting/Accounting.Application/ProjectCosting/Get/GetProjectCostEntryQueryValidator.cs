namespace Accounting.Application.ProjectCosting.Get;

/// <summary>
/// Validator for <see cref="GetProjectCostEntryQuery"/> to ensure valid identifiers are provided.
/// </summary>
public sealed class GetProjectCostEntryQueryValidator : AbstractValidator<GetProjectCostEntryQuery>
{
    public GetProjectCostEntryQueryValidator()
    {
        RuleFor(x => x.ProjectId)
            .NotEmpty()
            .Must(id => id != DefaultIdType.Empty)
            .WithMessage("ProjectId must be a non-empty GUID.");

        RuleFor(x => x.EntryId)
            .NotEmpty()
            .Must(id => id != DefaultIdType.Empty)
            .WithMessage("EntryId must be a non-empty GUID.");
    }
}


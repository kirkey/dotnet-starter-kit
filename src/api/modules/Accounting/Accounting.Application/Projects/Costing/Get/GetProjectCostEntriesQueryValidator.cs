namespace Accounting.Application.Projects.Costing.Get;

/// <summary>
/// Validator for <see cref="GetProjectCostEntriesQuery"/> ensuring a valid ProjectId is provided.
/// </summary>
public sealed class GetProjectCostEntriesQueryValidator : AbstractValidator<GetProjectCostEntriesQuery>
{
    public GetProjectCostEntriesQueryValidator()
    {
        RuleFor(x => x.ProjectId)
            .NotEmpty()
            .Must(id => id != DefaultIdType.Empty)
            .WithMessage("ProjectId must be a non-empty GUID.");
    }
}


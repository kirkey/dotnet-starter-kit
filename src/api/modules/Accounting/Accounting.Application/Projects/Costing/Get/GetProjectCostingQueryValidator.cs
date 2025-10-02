namespace Accounting.Application.Projects.Costing.Get;

/// <summary>
/// Validator for <see cref="GetProjectCostingQuery"/>.
/// </summary>
public sealed class GetProjectCostingQueryValidator : AbstractValidator<GetProjectCostingQuery>
{
    public GetProjectCostingQueryValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Project costing entry ID is required.");
    }
}

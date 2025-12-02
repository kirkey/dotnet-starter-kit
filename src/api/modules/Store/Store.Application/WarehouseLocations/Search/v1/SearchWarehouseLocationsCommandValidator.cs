namespace FSH.Starter.WebApi.Store.Application.WarehouseLocations.Search.v1;

public class SearchWarehouseLocationsCommandValidator : AbstractValidator<SearchWarehouseLocationsCommand>
{
    public SearchWarehouseLocationsCommandValidator()
    {
        RuleFor(x => x.PageNumber)
            .GreaterThanOrEqualTo(1)
            .WithMessage("PageNumber must be 1 or greater");

        RuleFor(x => x.PageSize)
            .GreaterThanOrEqualTo(1)
            .WithMessage("PageSize must be 1 or greater")
            .LessThanOrEqualTo(200)
            .WithMessage("PageSize must not exceed 200");

        RuleFor(x => x.SearchTerm)
            .MaximumLength(256)
            .WithMessage("Search term must not exceed 200 characters");

        RuleFor(x => x.LocationType)
            .MaximumLength(64)
            .WithMessage("LocationType must not exceed 50 characters");

        RuleFor(x => x.Aisle)
            .MaximumLength(32)
            .WithMessage("Aisle must not exceed 20 characters");

        // optional boolean filters - nothing to validate beyond nullability
    }
}

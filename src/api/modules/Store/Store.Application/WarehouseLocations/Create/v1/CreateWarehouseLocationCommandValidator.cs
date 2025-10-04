using FSH.Starter.WebApi.Store.Application.WarehouseLocations.Specs;

namespace FSH.Starter.WebApi.Store.Application.WarehouseLocations.Create.v1;

public class CreateWarehouseLocationCommandValidator : AbstractValidator<CreateWarehouseLocationCommand>
{
    public CreateWarehouseLocationCommandValidator(
        [FromKeyedServices("store:warehouse-locations")] IReadRepository<WarehouseLocation> repository,
        [FromKeyedServices("store:warehouses")] IReadRepository<Warehouse> warehouseRepository)
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("Location name is required")
            .MaximumLength(200)
            .WithMessage("Location name must not exceed 200 characters");

        RuleFor(x => x.Description)
            .MaximumLength(1000)
            .WithMessage("Description must not exceed 1000 characters");

        RuleFor(x => x.Code)
            .NotEmpty()
            .WithMessage("Location code is required")
            .MaximumLength(50)
            .WithMessage("Location code must not exceed 50 characters")
            .Matches(@"^[A-Z0-9\-]+$")
            .WithMessage("Location code must contain only uppercase letters, numbers and hyphens")
            .MustAsync(async (code, ct) =>
            {
                var existing = await repository.FirstOrDefaultAsync(new WarehouseLocationByCodeSpec(code), ct).ConfigureAwait(false);
                return existing is null;
            }).WithMessage("Location code must be unique");

        RuleFor(x => x.Aisle)
            .NotEmpty()
            .WithMessage("Aisle is required")
            .MaximumLength(20)
            .WithMessage("Aisle must not exceed 20 characters");

        RuleFor(x => x.Section)
            .NotEmpty()
            .WithMessage("Section is required")
            .MaximumLength(20)
            .WithMessage("Section must not exceed 20 characters");

        RuleFor(x => x.Shelf)
            .NotEmpty()
            .WithMessage("Shelf is required")
            .MaximumLength(20)
            .WithMessage("Shelf must not exceed 20 characters");

        RuleFor(x => x.Bin)
            .MaximumLength(20)
            .WithMessage("Bin must not exceed 20 characters");

        RuleFor(x => x.WarehouseId)
            .NotEmpty()
            .WithMessage("Warehouse ID is required")
            .MustAsync(async (id, ct) =>
            {
                var w = await warehouseRepository.GetByIdAsync(id, ct).ConfigureAwait(false);
                return w is not null;
            }).WithMessage("Warehouse not found");

        RuleFor(x => x.LocationType)
            .NotEmpty()
            .WithMessage("Location type is required")
            .MaximumLength(50)
            .WithMessage("Location type must not exceed 50 characters")
            .Must(type => new[] { "Floor", "Rack", "Cold Storage", "Freezer", "Dock", "Office" }.Contains(type))
            .WithMessage("Location type must be one of: Floor, Rack, Cold Storage, Freezer, Dock, Office");

        RuleFor(x => x.Capacity)
            .GreaterThan(0)
            .WithMessage("Capacity must be greater than 0");

        RuleFor(x => x.Notes)
            .MaximumLength(2048)
            .WithMessage("Notes must not exceed 2048 characters")
            .When(x => !string.IsNullOrEmpty(x.Notes));

        RuleFor(x => x.CapacityUnit)
            .NotEmpty()
            .WithMessage("Capacity unit is required")
            .MaximumLength(20)
            .WithMessage("Capacity unit must not exceed 20 characters")
            .Must(unit => new[] { "sqft", "sqm", "cbft", "cbm", "tons", "kg", "units" }.Contains(unit?.ToLower()))
            .WithMessage("Capacity unit must be one of: sqft, sqm, cbft, cbm, tons, kg, units");

        When(x => x.RequiresTemperatureControl, () =>
        {
            RuleFor(x => x.MinTemperature)
                .NotNull()
                .WithMessage("Minimum temperature is required when temperature control is enabled");

            RuleFor(x => x.MaxTemperature)
                .NotNull()
                .WithMessage("Maximum temperature is required when temperature control is enabled")
                .GreaterThan(x => x.MinTemperature)
                .WithMessage("Maximum temperature must be greater than minimum temperature");

            RuleFor(x => x.TemperatureUnit)
                .NotEmpty()
                .WithMessage("Temperature unit is required when temperature control is enabled")
                .Must(unit => new[] { "C", "F" }.Contains(unit))
                .WithMessage("Temperature unit must be either 'C' or 'F'");
        });
    }
}

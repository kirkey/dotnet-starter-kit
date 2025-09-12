namespace FSH.Starter.WebApi.Store.Application.WarehouseLocations.Create.v1;

public class CreateWarehouseLocationCommandValidator : AbstractValidator<CreateWarehouseLocationCommand>
{
    public CreateWarehouseLocationCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("Location name is required")
            .MaximumLength(100)
            .WithMessage("Location name must not exceed 100 characters");

        RuleFor(x => x.Code)
            .NotEmpty()
            .WithMessage("Location code is required")
            .MaximumLength(20)
            .WithMessage("Location code must not exceed 20 characters")
            .Matches(@"^[A-Z0-9]+$")
            .WithMessage("Location code must contain only uppercase letters and numbers");

        RuleFor(x => x.Aisle)
            .NotEmpty()
            .WithMessage("Aisle is required")
            .MaximumLength(10)
            .WithMessage("Aisle must not exceed 10 characters");

        RuleFor(x => x.Section)
            .NotEmpty()
            .WithMessage("Section is required")
            .MaximumLength(10)
            .WithMessage("Section must not exceed 10 characters");

        RuleFor(x => x.Shelf)
            .NotEmpty()
            .WithMessage("Shelf is required")
            .MaximumLength(10)
            .WithMessage("Shelf must not exceed 10 characters");

        RuleFor(x => x.WarehouseId)
            .NotEmpty()
            .WithMessage("Warehouse ID is required");

        RuleFor(x => x.LocationType)
            .NotEmpty()
            .WithMessage("Location type is required")
            .Must(type => new[] { "Floor", "Rack", "Cold Storage", "Freezer", "Dock", "Office" }.Contains(type))
            .WithMessage("Location type must be one of: Floor, Rack, Cold Storage, Freezer, Dock, Office");

        RuleFor(x => x.Capacity)
            .GreaterThan(0)
            .WithMessage("Capacity must be greater than 0");

        RuleFor(x => x.CapacityUnit)
            .NotEmpty()
            .WithMessage("Capacity unit is required")
            .Must(unit => new[] { "sqft", "sqm", "cbft", "cbm", "tons", "kg" }.Contains(unit.ToLower()))
            .WithMessage("Capacity unit must be one of: sqft, sqm, cbft, cbm, tons, kg");

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

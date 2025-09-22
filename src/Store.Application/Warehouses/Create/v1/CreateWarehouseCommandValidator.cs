namespace FSH.Starter.WebApi.Store.Application.Warehouses.Create.v1;

public class CreateWarehouseCommandValidator : AbstractValidator<CreateWarehouseCommand>
{
    public CreateWarehouseCommandValidator([FromKeyedServices("store:warehouses")] IReadRepository<Warehouse> repository)
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("Warehouse name is required")
            .MaximumLength(200)
            .WithMessage("Warehouse name must not exceed 200 characters");

        RuleFor(x => x.Code)
            .NotEmpty()
            .WithMessage("Warehouse code is required")
            .MaximumLength(50)
            .WithMessage("Warehouse code must not exceed 50 characters")
            .Matches(@"^[A-Z0-9\-]+$")
            .WithMessage("Warehouse code must contain only uppercase letters, numbers, and hyphens")
            .MustAsync(async (code, ct) =>
            {
                var existing = await repository.FirstOrDefaultAsync(new WarehouseByCodeSpec(code), ct).ConfigureAwait(false);
                return existing is null;
            }).WithMessage("Warehouse code must be unique");

        RuleFor(x => x.Address)
            .NotEmpty()
            .WithMessage("Address is required")
            .MaximumLength(500)
            .WithMessage("Address must not exceed 500 characters");

        RuleFor(x => x.City)
            .NotEmpty()
            .WithMessage("City is required")
            .MaximumLength(100)
            .WithMessage("City must not exceed 100 characters");

        RuleFor(x => x.State)
            .MaximumLength(100)
            .WithMessage("State must not exceed 100 characters")
            .When(x => !string.IsNullOrEmpty(x.State));

        RuleFor(x => x.Country)
            .NotEmpty()
            .WithMessage("Country is required")
            .MaximumLength(100)
            .WithMessage("Country must not exceed 100 characters");

        RuleFor(x => x.PostalCode)
            .MaximumLength(20)
            .WithMessage("Postal code must not exceed 20 characters")
            .When(x => !string.IsNullOrEmpty(x.PostalCode));

        RuleFor(x => x.ManagerName)
            .NotEmpty()
            .WithMessage("Manager name is required")
            .MaximumLength(100)
            .WithMessage("Manager name must not exceed 100 characters");

        RuleFor(x => x.ManagerEmail)
            .NotEmpty()
            .WithMessage("Manager email is required")
            .MaximumLength(255)
            .WithMessage("Manager email must not exceed 255 characters")
            .EmailAddress()
            .WithMessage("Manager email must be a valid email address");

        RuleFor(x => x.ManagerPhone)
            .NotEmpty()
            .WithMessage("Manager phone is required")
            .MaximumLength(50)
            .WithMessage("Manager phone must not exceed 50 characters");

        RuleFor(x => x.TotalCapacity)
            .GreaterThan(0)
            .WithMessage("Total capacity must be greater than 0");

        RuleFor(x => x.CapacityUnit)
            .NotEmpty()
            .WithMessage("Capacity unit is required")
            .MaximumLength(20)
            .WithMessage("Capacity unit must not exceed 20 characters")
            .Must(unit => new[] { "sqft", "sqm", "cbft", "cbm", "tons", "kg", "pallets" }.Contains(unit.ToLower()))
            .WithMessage("Capacity unit must be one of: sqft, sqm, cbft, cbm, tons, kg, pallets");

        RuleFor(x => x.WarehouseType)
            .NotEmpty()
            .WithMessage("Warehouse type is required")
            .MaximumLength(50)
            .WithMessage("Warehouse type must not exceed 50 characters")
            .Must(type => new[] { "standard", "cold storage", "frozen", "hazmat", "pharmaceutical", "automotive" }.Contains(type.ToLower()))
            .WithMessage("Warehouse type must be one of: Standard, Cold Storage, Frozen, Hazmat, Pharmaceutical, Automotive");
    }
}

public class WarehouseByCodeSpec : Specification<Warehouse>
{
    public WarehouseByCodeSpec(string code)
    {
        Query.Where(w => w.Code == code);
    }
}

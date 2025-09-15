namespace FSH.Starter.WebApi.Store.Application.Warehouses.Update.v1;

public class UpdateWarehouseCommandValidator : AbstractValidator<UpdateWarehouseCommand>
{
    public UpdateWarehouseCommandValidator([FromKeyedServices("store:warehouses")] IReadRepository<Warehouse> repository)
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Warehouse ID is required");

        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("Warehouse name is required")
            .MaximumLength(100)
            .WithMessage("Warehouse name must not exceed 100 characters");

        RuleFor(x => x.Code)
            .NotEmpty()
            .WithMessage("Warehouse code is required")
            .MaximumLength(20)
            .WithMessage("Warehouse code must not exceed 20 characters")
            .Matches(@"^[A-Z0-9]+$")
            .WithMessage("Warehouse code must contain only uppercase letters and numbers")
            .MustAsync(async (cmd, code, ct) =>
            {
                var existing = await repository.FirstOrDefaultAsync(new WarehouseByCodeSpec(code), ct).ConfigureAwait(false);
                // allow if no existing or existing is the same entity
                return existing is null || existing.Id == cmd.Id;
            }).WithMessage("Warehouse code must be unique");

        RuleFor(x => x.Address)
            .NotEmpty()
            .WithMessage("Address is required")
            .MaximumLength(200)
            .WithMessage("Address must not exceed 200 characters");

        RuleFor(x => x.City)
            .NotEmpty()
            .WithMessage("City is required")
            .MaximumLength(100)
            .WithMessage("City must not exceed 100 characters");

        RuleFor(x => x.Country)
            .NotEmpty()
            .WithMessage("Country is required")
            .MaximumLength(100)
            .WithMessage("Country must not exceed 100 characters");

        RuleFor(x => x.ManagerName)
            .NotEmpty()
            .WithMessage("Manager name is required")
            .MaximumLength(100)
            .WithMessage("Manager name must not exceed 100 characters");

        RuleFor(x => x.ManagerEmail)
            .NotEmpty()
            .WithMessage("Manager email is required")
            .EmailAddress()
            .WithMessage("Manager email must be a valid email address");

        RuleFor(x => x.ManagerPhone)
            .NotEmpty()
            .WithMessage("Manager phone is required")
            .MaximumLength(20)
            .WithMessage("Manager phone must not exceed 20 characters");

        RuleFor(x => x.TotalCapacity)
            .GreaterThan(0)
            .WithMessage("Total capacity must be greater than 0");

        RuleFor(x => x.CapacityUnit)
            .NotEmpty()
            .WithMessage("Capacity unit is required")
            .Must(unit => new[] { "sqft", "sqm", "cbft", "cbm", "tons", "kg" }.Contains(unit.ToLower()))
            .WithMessage("Capacity unit must be one of: sqft, sqm, cbft, cbm, tons, kg");
    }
}

// Local spec re-used in validators
public class WarehouseByCodeSpec : Specification<Warehouse>
{
    public WarehouseByCodeSpec(string code)
    {
        Query.Where(w => w.Code == code);
    }
}

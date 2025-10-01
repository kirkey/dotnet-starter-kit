namespace FSH.Starter.WebApi.Store.Application.Warehouses.AssignManager.v1;

public class AssignWarehouseManagerCommandValidator : AbstractValidator<AssignWarehouseManagerCommand>
{
    public AssignWarehouseManagerCommandValidator([FromKeyedServices("store:warehouses")] IReadRepository<Warehouse> repository)
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Warehouse ID is required")
            .MustAsync(async (id, ct) =>
            {
                var w = await repository.GetByIdAsync(id, ct).ConfigureAwait(false);
                return w is not null;
            }).WithMessage("Warehouse not found");

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
    }
}


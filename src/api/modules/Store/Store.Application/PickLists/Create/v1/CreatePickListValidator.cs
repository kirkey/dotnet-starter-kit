namespace FSH.Starter.WebApi.Store.Application.PickLists.Create.v1;

public sealed class CreatePickListValidator : AbstractValidator<CreatePickListCommand>
{
    public CreatePickListValidator()
    {
        RuleFor(x => x.PickListNumber)
            .NotEmpty().WithMessage("Pick list number is required")
            .MaximumLength(128).WithMessage("Pick list number must not exceed 100 characters");

        RuleFor(x => x.WarehouseId)
            .NotEmpty().WithMessage("Warehouse ID is required");

        RuleFor(x => x.PickingType)
            .NotEmpty().WithMessage("Picking type is required")
            .Must(type => new[] { "Order", "Wave", "Batch", "Zone" }.Contains(type, StringComparer.OrdinalIgnoreCase))
            .WithMessage("Picking type must be one of: Order, Wave, Batch, Zone");

        RuleFor(x => x.ReferenceNumber)
            .MaximumLength(128).WithMessage("Reference number must not exceed 100 characters")
            .When(x => !string.IsNullOrWhiteSpace(x.ReferenceNumber));

        RuleFor(x => x.Name)
            .MaximumLength(1024).WithMessage("Name must not exceed 1024 characters")
            .When(x => !string.IsNullOrWhiteSpace(x.Name));

        RuleFor(x => x.Description)
            .MaximumLength(2048).WithMessage("Description must not exceed 2048 characters")
            .When(x => !string.IsNullOrWhiteSpace(x.Description));

        RuleFor(x => x.Notes)
            .MaximumLength(2048).WithMessage("Notes must not exceed 2048 characters")
            .When(x => !string.IsNullOrWhiteSpace(x.Notes));
    }
}

namespace FSH.Starter.WebApi.Store.Application.PutAwayTasks.Create.v1;

public sealed class CreatePutAwayTaskValidator : AbstractValidator<CreatePutAwayTaskCommand>
{
    public CreatePutAwayTaskValidator()
    {
        RuleFor(x => x.TaskNumber)
            .NotEmpty().WithMessage("Task number is required")
            .MaximumLength(128).WithMessage("Task number must not exceed 100 characters");

        RuleFor(x => x.WarehouseId)
            .NotEmpty().WithMessage("Warehouse ID is required");

        RuleFor(x => x.PutAwayStrategy)
            .MaximumLength(64).WithMessage("Put-away strategy must not exceed 50 characters")
            .When(x => !string.IsNullOrWhiteSpace(x.PutAwayStrategy));

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

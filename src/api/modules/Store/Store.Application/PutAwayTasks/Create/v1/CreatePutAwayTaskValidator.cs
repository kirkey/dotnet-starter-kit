namespace FSH.Starter.WebApi.Store.Application.PutAwayTasks.Create.v1;

public sealed class CreatePutAwayTaskValidator : AbstractValidator<CreatePutAwayTaskCommand>
{
    public CreatePutAwayTaskValidator()
    {
        RuleFor(x => x.TaskNumber)
            .NotEmpty().WithMessage("Task number is required")
            .MaximumLength(100).WithMessage("Task number must not exceed 100 characters");

        RuleFor(x => x.WarehouseId)
            .NotEmpty().WithMessage("Warehouse ID is required");

        RuleFor(x => x.PutAwayStrategy)
            .MaximumLength(50).WithMessage("Put-away strategy must not exceed 50 characters")
            .When(x => !string.IsNullOrWhiteSpace(x.PutAwayStrategy));

        RuleFor(x => x.Notes)
            .MaximumLength(500).WithMessage("Notes must not exceed 500 characters")
            .When(x => !string.IsNullOrWhiteSpace(x.Notes));
    }
}

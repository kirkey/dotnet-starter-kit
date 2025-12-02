namespace FSH.Starter.WebApi.Store.Application.CycleCounts.Create.v1;

public class CreateCycleCountCommandValidator : AbstractValidator<CreateCycleCountCommand>
{
    private static readonly string[] AllowedCountTypes = new[] { "Full", "Partial", "ABC", "Random" };

    public CreateCycleCountCommandValidator()
    {
        RuleFor(x => x.CountNumber)
            .NotEmpty()
            .MaximumLength(256)
            .Matches(@"^[A-Z0-9\-]+$")
            .WithMessage("CountNumber must contain only uppercase letters, numbers or hyphens");

        RuleFor(x => x.WarehouseId)
            .NotEmpty();

        RuleFor(x => x.WarehouseLocationId)
            .NotEmpty()
            .When(x => x.WarehouseLocationId.HasValue);

        RuleFor(x => x.ScheduledDate)
            .Must(d => d.Date >= DateTime.UtcNow.Date)
            .WithMessage("Scheduled date cannot be in the past");

        RuleFor(x => x.CountType)
            .NotEmpty()
            .Must(ct => AllowedCountTypes.Contains(ct))
            .WithMessage($"CountType must be one of: {string.Join(", ", AllowedCountTypes)}");

        RuleFor(x => x.CounterName)
            .MaximumLength(128)
            .When(x => !string.IsNullOrEmpty(x.CounterName));

        RuleFor(x => x.SupervisorName)
            .MaximumLength(128)
            .When(x => !string.IsNullOrEmpty(x.SupervisorName));

        RuleFor(x => x.Name)
            .MaximumLength(1024)
            .WithMessage("Name must not exceed 1024 characters")
            .When(x => !string.IsNullOrEmpty(x.Name));

        RuleFor(x => x.Description)
            .MaximumLength(2048)
            .WithMessage("Description must not exceed 2048 characters")
            .When(x => !string.IsNullOrEmpty(x.Description));

        RuleFor(x => x.Notes)
            .MaximumLength(2048)
            .WithMessage("Notes must not exceed 2048 characters")
            .When(x => !string.IsNullOrEmpty(x.Notes));
    }
}

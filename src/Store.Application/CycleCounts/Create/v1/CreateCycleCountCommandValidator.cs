namespace FSH.Starter.WebApi.Store.Application.CycleCounts.Create.v1;

public class CreateCycleCountCommandValidator : AbstractValidator<CreateCycleCountCommand>
{
    private static readonly string[] AllowedCountTypes = new[] { "Full", "Partial", "ABC", "Random" };

    public CreateCycleCountCommandValidator()
    {
        RuleFor(x => x.CountNumber)
            .NotEmpty()
            .MaximumLength(200)
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
            .MaximumLength(100)
            .When(x => !string.IsNullOrEmpty(x.CounterName));

        RuleFor(x => x.SupervisorName)
            .MaximumLength(100)
            .When(x => !string.IsNullOrEmpty(x.SupervisorName));

        RuleFor(x => x.Notes)
            .MaximumLength(1024)
            .When(x => !string.IsNullOrEmpty(x.Notes));
    }
}

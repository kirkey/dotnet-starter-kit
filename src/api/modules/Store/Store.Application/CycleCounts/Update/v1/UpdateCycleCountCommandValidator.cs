namespace FSH.Starter.WebApi.Store.Application.CycleCounts.Update.v1;

/// <summary>
/// Validator for UpdateCycleCountCommand with stricter validation rules.
/// Ensures data integrity and business rule compliance for cycle count updates.
/// </summary>
public class UpdateCycleCountCommandValidator : AbstractValidator<UpdateCycleCountCommand>
{
    private static readonly string[] AllowedCountTypes = new[] { "Full", "Partial", "ABC", "Random" };

    public UpdateCycleCountCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Cycle count ID is required");

        RuleFor(x => x.WarehouseId)
            .NotEmpty()
            .WithMessage("Warehouse ID is required");

        RuleFor(x => x.WarehouseLocationId)
            .NotEmpty()
            .When(x => x.WarehouseLocationId.HasValue)
            .WithMessage("Warehouse location ID must be valid when provided");

        RuleFor(x => x.ScheduledDate)
            .NotEmpty()
            .WithMessage("Scheduled date is required")
            .Must(d => d.Date >= DateTime.UtcNow.Date)
            .WithMessage("Scheduled date cannot be in the past");

        RuleFor(x => x.CountType)
            .NotEmpty()
            .WithMessage("Count type is required")
            .MaximumLength(50)
            .WithMessage("Count type must not exceed 50 characters")
            .Must(ct => AllowedCountTypes.Contains(ct))
            .WithMessage($"CountType must be one of: {string.Join(", ", AllowedCountTypes)}");

        RuleFor(x => x.CounterName)
            .MaximumLength(100)
            .WithMessage("Counter name must not exceed 100 characters")
            .When(x => !string.IsNullOrEmpty(x.CounterName));

        RuleFor(x => x.SupervisorName)
            .MaximumLength(100)
            .WithMessage("Supervisor name must not exceed 100 characters")
            .When(x => !string.IsNullOrEmpty(x.SupervisorName));

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


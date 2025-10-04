namespace FSH.Starter.WebApi.Store.Application.LotNumbers.Update.v1;

/// <summary>
/// Validator for UpdateLotNumberCommand.
/// </summary>
public sealed class UpdateLotNumberCommandValidator : AbstractValidator<UpdateLotNumberCommand>
{
    public UpdateLotNumberCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Id is required");

        RuleFor(x => x.Status)
            .Must(status => new[] { "Active", "Expired", "Quarantine", "Recalled" }.Contains(status, StringComparer.OrdinalIgnoreCase))
            .WithMessage("Status must be one of: Active, Expired, Quarantine, Recalled")
            .When(x => !string.IsNullOrWhiteSpace(x.Status));

        RuleFor(x => x.QualityNotes)
            .MaximumLength(1000)
            .WithMessage("QualityNotes must not exceed 1000 characters")
            .When(x => !string.IsNullOrWhiteSpace(x.QualityNotes));

        RuleFor(x => x.Name)
            .MaximumLength(1024)
            .WithMessage("Name must not exceed 1024 characters")
            .When(x => !string.IsNullOrWhiteSpace(x.Name));

        RuleFor(x => x.Description)
            .MaximumLength(2048)
            .WithMessage("Description must not exceed 2048 characters")
            .When(x => !string.IsNullOrWhiteSpace(x.Description));

        RuleFor(x => x.Notes)
            .MaximumLength(2048)
            .WithMessage("Notes must not exceed 2048 characters")
            .When(x => !string.IsNullOrWhiteSpace(x.Notes));
    }
}

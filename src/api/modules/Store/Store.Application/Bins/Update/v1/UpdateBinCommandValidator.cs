namespace FSH.Starter.WebApi.Store.Application.Bins.Update.v1;

public class UpdateBinCommandValidator : AbstractValidator<UpdateBinCommand>
{
    public UpdateBinCommandValidator()
    {
        RuleFor(c => c.Id)
            .NotEmpty().WithMessage("Id is required.");

        RuleFor(c => c.Name)
            .MaximumLength(200).When(c => !string.IsNullOrWhiteSpace(c.Name))
            .WithMessage("Name must not exceed 200 characters.");

        RuleFor(c => c.BinType)
            .MaximumLength(50).When(c => !string.IsNullOrWhiteSpace(c.BinType))
            .WithMessage("BinType must not exceed 50 characters.");

        RuleFor(c => c.Capacity)
            .GreaterThanOrEqualTo(0).When(c => c.Capacity.HasValue)
            .WithMessage("Capacity cannot be negative.");

        RuleFor(c => c.Priority)
            .GreaterThanOrEqualTo(0).When(c => c.Priority.HasValue)
            .WithMessage("Priority cannot be negative.");

        RuleFor(c => c.Description)
            .MaximumLength(2048).When(c => !string.IsNullOrWhiteSpace(c.Description))
            .WithMessage("Description must not exceed 2048 characters.");

        RuleFor(c => c.Notes)
            .MaximumLength(2048).When(c => !string.IsNullOrWhiteSpace(c.Notes))
            .WithMessage("Notes must not exceed 2048 characters.");
    }
}

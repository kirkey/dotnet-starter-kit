namespace FSH.Starter.WebApi.Store.Application.InventoryTransactions.UpdateNotes.v1;

/// <summary>
/// Validator for UpdateInventoryTransactionNotesCommand.
/// </summary>
public class UpdateInventoryTransactionNotesValidator : AbstractValidator<UpdateInventoryTransactionNotesCommand>
{
    public UpdateInventoryTransactionNotesValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Transaction ID is required.");

        RuleFor(x => x.Notes)
            .MaximumLength(2048)
            .When(x => !string.IsNullOrWhiteSpace(x.Notes))
            .WithMessage("Notes must not exceed 2048 characters.");
    }
}


namespace FSH.Starter.WebApi.Store.Application.InventoryTransactions.Create.v1;

public class CreateInventoryTransactionValidator : AbstractValidator<CreateInventoryTransactionCommand>
{
    public CreateInventoryTransactionValidator()
    {
        RuleFor(x => x.TransactionNumber)
            .NotEmpty().WithMessage("Transaction number is required.")
            .MaximumLength(128).WithMessage("Transaction number must not exceed 100 characters.");

        RuleFor(x => x.ItemId)
            .NotEmpty().WithMessage("Item ID is required.");

        RuleFor(x => x.TransactionType)
            .NotEmpty().WithMessage("Transaction type is required.")
            .Must(type => new[] { "IN", "OUT", "ADJUSTMENT", "TRANSFER" }.Contains(type))
            .WithMessage("Transaction type must be one of: IN, OUT, ADJUSTMENT, TRANSFER.");

        RuleFor(x => x.Reason)
            .NotEmpty().WithMessage("Reason is required.")
            .MaximumLength(256).WithMessage("Reason must not exceed 200 characters.");

        RuleFor(x => x.Quantity)
            .GreaterThan(0).WithMessage("Quantity must be greater than zero.");

        RuleFor(x => x.QuantityBefore)
            .GreaterThanOrEqualTo(0).WithMessage("Quantity before must be zero or greater.");

        RuleFor(x => x.UnitCost)
            .GreaterThanOrEqualTo(0).WithMessage("Unit cost must be zero or greater.");

        RuleFor(x => x.TransactionDate)
            .NotEmpty().WithMessage("Transaction date is required.")
            .LessThanOrEqualTo(DateTime.UtcNow).WithMessage("Transaction date cannot be in the future.");

        RuleFor(x => x.Name)
            .MaximumLength(1024).When(x => !string.IsNullOrWhiteSpace(x.Name));

        RuleFor(x => x.Description)
            .MaximumLength(2048).When(x => !string.IsNullOrWhiteSpace(x.Description));

        RuleFor(x => x.Notes)
            .MaximumLength(2048).When(x => !string.IsNullOrWhiteSpace(x.Notes));
    }
}

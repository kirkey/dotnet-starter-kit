namespace Accounting.Application.Bills.LineItems.Delete.v1;

/// <summary>
/// Validator for DeleteBillLineItemCommand.
/// </summary>
public sealed class DeleteBillLineItemCommandValidator : AbstractValidator<DeleteBillLineItemCommand>
{
    public DeleteBillLineItemCommandValidator()
    {
        RuleFor(x => x.LineItemId)
            .NotEmpty()
            .WithMessage("Line item ID is required.")
            .Must(id => id != DefaultIdType.Empty)
            .WithMessage("Line item ID must be a valid identifier.");

        RuleFor(x => x.BillId)
            .NotEmpty()
            .WithMessage("Bill ID is required.")
            .Must(id => id != DefaultIdType.Empty)
            .WithMessage("Bill ID must be a valid identifier.");
    }
}


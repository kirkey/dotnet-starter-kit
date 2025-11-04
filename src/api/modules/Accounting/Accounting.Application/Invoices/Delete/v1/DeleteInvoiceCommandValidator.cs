namespace Accounting.Application.Invoices.Delete.v1;

/// <summary>
/// Validator for delete invoice command.
/// </summary>
public class DeleteInvoiceCommandValidator : AbstractValidator<DeleteInvoiceCommand>
{
    public DeleteInvoiceCommandValidator()
    {
        RuleFor(x => x.InvoiceId)
            .NotEmpty()
            .WithMessage("Invoice ID is required");
    }
}


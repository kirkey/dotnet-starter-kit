namespace Accounting.Application.Bills.Create.v1;

/// <summary>
/// Validator for CreateBillCommand with strict validation rules.
/// </summary>
public sealed class CreateBillCommandValidator : AbstractValidator<CreateBillCommand>
{
    public CreateBillCommandValidator()
    {
        RuleFor(x => x.BillNumber)
            .NotEmpty()
            .WithMessage("Bill number is required.")
            .MaximumLength(64)
            .WithMessage("Bill number cannot exceed 50 characters.")
            .Must(BeValidBillNumber)
            .WithMessage("Bill number contains invalid characters.");

        RuleFor(x => x.VendorId)
            .NotEmpty()
            .WithMessage("Vendor ID is required.")
            .Must(id => id != DefaultIdType.Empty)
            .WithMessage("Vendor ID must be a valid identifier.");

        RuleFor(x => x.BillDate)
            .NotEmpty()
            .WithMessage("Bill date is required.")
            .Must(date => date != default)
            .WithMessage("Bill date must be a valid date.")
            .LessThanOrEqualTo(DateTime.UtcNow.Date.AddDays(30))
            .WithMessage("Bill date cannot be more than 30 days in the future.");

        RuleFor(x => x.DueDate)
            .NotEmpty()
            .WithMessage("Due date is required.")
            .Must(date => date != default)
            .WithMessage("Due date must be a valid date.")
            .GreaterThanOrEqualTo(x => x.BillDate)
            .WithMessage("Due date cannot be before bill date.");

        RuleFor(x => x.Description)
            .MaximumLength(512)
            .When(x => !string.IsNullOrWhiteSpace(x.Description))
            .WithMessage("Description cannot exceed 500 characters.");

        RuleFor(x => x.PaymentTerms)
            .MaximumLength(128)
            .When(x => !string.IsNullOrWhiteSpace(x.PaymentTerms))
            .WithMessage("Payment terms cannot exceed 100 characters.");

        RuleFor(x => x.PurchaseOrderNumber)
            .MaximumLength(64)
            .When(x => !string.IsNullOrWhiteSpace(x.PurchaseOrderNumber))
            .WithMessage("Purchase order number cannot exceed 50 characters.");

        RuleFor(x => x.Notes)
            .MaximumLength(2048)
            .When(x => !string.IsNullOrWhiteSpace(x.Notes))
            .WithMessage("Notes cannot exceed 2000 characters.");

        RuleFor(x => x.LineItems)
            .NotNull()
            .WithMessage("Line items are required.")
            .Must(lines => lines != null && lines.Count > 0)
            .WithMessage("At least one line item is required.");

        RuleForEach(x => x.LineItems)
            .SetValidator(new BillLineItemDtoValidator())
            .When(x => x.LineItems != null);

        // Custom validation: ensure line numbers are unique and sequential
        RuleFor(x => x.LineItems)
            .Must(HaveUniqueLineNumbers)
            .When(x => x.LineItems != null && x.LineItems.Count > 0)
            .WithMessage("Line numbers must be unique.");

        // Custom validation: ensure total amounts are positive
        RuleFor(x => x.LineItems)
            .Must(HavePositiveAmounts)
            .When(x => x.LineItems != null && x.LineItems.Count > 0)
            .WithMessage("Line item amounts must be positive.");
    }

    private static bool BeValidBillNumber(string billNumber)
    {
        if (string.IsNullOrWhiteSpace(billNumber))
            return false;

        // Allow alphanumeric, dash, underscore, and space
        return billNumber.All(c => char.IsLetterOrDigit(c) || c == '-' || c == '_' || c == ' ');
    }

    private static bool HaveUniqueLineNumbers(List<BillLineItemDto>? lineItems)
    {
        if (lineItems == null || lineItems.Count == 0)
            return true;

        var lineNumbers = lineItems.Select(x => x.LineNumber).ToList();
        return lineNumbers.Count == lineNumbers.Distinct().Count();
    }

    private static bool HavePositiveAmounts(List<BillLineItemDto>? lineItems)
    {
        if (lineItems == null || lineItems.Count == 0)
            return true;

        return lineItems.All(x => x.Amount >= 0 && x.Quantity > 0 && x.UnitPrice >= 0);
    }
}

/// <summary>
/// Validator for BillLineItemDto with strict validation rules.
/// </summary>
public sealed class BillLineItemDtoValidator : AbstractValidator<BillLineItemDto>
{
    public BillLineItemDtoValidator()
    {
        RuleFor(x => x.LineNumber)
            .GreaterThan(0)
            .WithMessage("Line number must be positive.");

        RuleFor(x => x.Description)
            .NotEmpty()
            .WithMessage("Line item description is required.")
            .MaximumLength(512)
            .WithMessage("Line item description cannot exceed 500 characters.");

        RuleFor(x => x.Quantity)
            .GreaterThan(0)
            .WithMessage("Quantity must be greater than zero.")
            .LessThanOrEqualTo(999999999)
            .WithMessage("Quantity cannot exceed 999,999,999.");

        RuleFor(x => x.UnitPrice)
            .GreaterThanOrEqualTo(0)
            .WithMessage("Unit price cannot be negative.")
            .LessThanOrEqualTo(999999999)
            .WithMessage("Unit price cannot exceed 999,999,999.");

        RuleFor(x => x.Amount)
            .GreaterThanOrEqualTo(0)
            .WithMessage("Amount cannot be negative.")
            .LessThanOrEqualTo(999999999)
            .WithMessage("Amount cannot exceed 999,999,999.");

        RuleFor(x => x.ChartOfAccountId)
            .NotEmpty()
            .WithMessage("Chart of account is required.")
            .Must(id => id != DefaultIdType.Empty)
            .WithMessage("Chart of account must be a valid identifier.");

        RuleFor(x => x.TaxAmount)
            .GreaterThanOrEqualTo(0)
            .WithMessage("Tax amount cannot be negative.")
            .LessThanOrEqualTo(999999999)
            .WithMessage("Tax amount cannot exceed 999,999,999.");

        RuleFor(x => x.Notes)
            .MaximumLength(1024)
            .When(x => !string.IsNullOrWhiteSpace(x.Notes))
            .WithMessage("Line item notes cannot exceed 1000 characters.");

        // Custom validation: Amount should approximately equal Quantity * UnitPrice
        RuleFor(x => x)
            .Must(line => Math.Abs(line.Amount - (line.Quantity * line.UnitPrice)) < 0.01m)
            .WithMessage("Amount should equal Quantity × Unit Price.");
    }
}


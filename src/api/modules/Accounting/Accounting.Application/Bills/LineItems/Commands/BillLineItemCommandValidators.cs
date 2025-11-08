using Accounting.Application.Bills.LineItems.Create.v1;
using Accounting.Application.Bills.LineItems.Delete.v1;
using Accounting.Application.Bills.LineItems.Update.v1;

namespace Accounting.Application.Bills.LineItems.Commands;

/// <summary>
/// Validator for AddBillLineItemCommand with strict validation rules.
/// </summary>
public sealed class AddBillLineItemCommandValidator : AbstractValidator<AddBillLineItemCommand>
{
    public AddBillLineItemCommandValidator()
    {
        RuleFor(x => x.BillId)
            .NotEmpty()
            .WithMessage("Bill ID is required.")
            .Must(id => id != DefaultIdType.Empty)
            .WithMessage("Bill ID must be a valid identifier.");

        RuleFor(x => x.LineNumber)
            .GreaterThan(0)
            .WithMessage("Line number must be positive.")
            .LessThanOrEqualTo(9999)
            .WithMessage("Line number cannot exceed 9999.");

        RuleFor(x => x.Description)
            .NotEmpty()
            .WithMessage("Description is required.")
            .MaximumLength(500)
            .WithMessage("Description cannot exceed 500 characters.");

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
            .MaximumLength(1000)
            .When(x => !string.IsNullOrWhiteSpace(x.Notes))
            .WithMessage("Notes cannot exceed 1000 characters.");

        // Custom validation: Amount should approximately equal Quantity * UnitPrice
        RuleFor(x => x)
            .Must(cmd => Math.Abs(cmd.Amount - (cmd.Quantity * cmd.UnitPrice)) < 0.01m)
            .WithMessage("Amount should equal Quantity × Unit Price.");
    }
}

/// <summary>
/// Validator for UpdateBillLineItemCommand with strict validation rules.
/// </summary>
public sealed class UpdateBillLineItemCommandValidator : AbstractValidator<UpdateBillLineItemCommand>
{
    public UpdateBillLineItemCommandValidator()
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

        RuleFor(x => x.LineNumber)
            .GreaterThan(0)
            .WithMessage("Line number must be positive.")
            .LessThanOrEqualTo(9999)
            .WithMessage("Line number cannot exceed 9999.");

        RuleFor(x => x.Description)
            .NotEmpty()
            .WithMessage("Description is required.")
            .MaximumLength(500)
            .WithMessage("Description cannot exceed 500 characters.");

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

        RuleFor(x => x.TaxAmount)
            .GreaterThanOrEqualTo(0)
            .WithMessage("Tax amount cannot be negative.")
            .LessThanOrEqualTo(999999999)
            .WithMessage("Tax amount cannot exceed 999,999,999.");

        RuleFor(x => x.Notes)
            .MaximumLength(1000)
            .When(x => !string.IsNullOrWhiteSpace(x.Notes))
            .WithMessage("Notes cannot exceed 1000 characters.");
    }
}

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


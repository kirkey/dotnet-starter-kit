using Accounting.Domain.Events.Invoice;

namespace Accounting.Domain;

/// <summary>
/// Represents a customer/member invoice including usage-based and fixed charges, taxes, fees, and line items.
/// </summary>
/// <remarks>
/// Tracks billing period, kWh usage, status lifecycle (Draft, Sent, Paid, Overdue, Cancelled), payments applied, and totals.
/// Defaults: <see cref="Status"/> "Draft"; <see cref="PaidAmount"/> 0; string properties trimmed.
/// </remarks>
public class Invoice : AuditableEntity, IAggregateRoot
{
    /// <summary>
    /// Unique invoice number.
    /// </summary>
    public string InvoiceNumber { get; private set; } // InvoiceID equivalent

    /// <summary>
    /// Identifier of the billed member.
    /// </summary>
    public DefaultIdType MemberId { get; private set; } // Foreign Key to Member

    /// <summary>
    /// Date when the invoice was created.
    /// </summary>
    public DateTime InvoiceDate { get; private set; }

    /// <summary>
    /// Payment due date; must be on/after <see cref="InvoiceDate"/>.
    /// </summary>
    public DateTime DueDate { get; private set; }

    /// <summary>
    /// Total amount billed across all components and line items.
    /// </summary>
    public decimal TotalAmount { get; private set; }

    /// <summary>
    /// Amount paid towards this invoice.
    /// </summary>
    public decimal PaidAmount { get; private set; }

    /// <summary>
    /// Workflow status: Draft, Sent, Paid, Overdue, Cancelled.
    /// </summary>
    public string Status { get; private set; } // "Draft", "Sent", "Paid", "Overdue", "Cancelled"

    /// <summary>
    /// Optional link to the consumption record that informed this invoice.
    /// </summary>
    public DefaultIdType? ConsumptionId { get; private set; } // Links to consumption record

    /// <summary>
    /// Usage-based charge (e.g., energy portion) for the period.
    /// </summary>
    public decimal UsageCharge { get; private set; } // Charge based on kWhUsed

    /// <summary>
    /// Fixed monthly charge component.
    /// </summary>
    public decimal BasicServiceCharge { get; private set; } // Fixed monthly charge

    /// <summary>
    /// Total tax amount.
    /// </summary>
    public decimal TaxAmount { get; private set; } // Tax amount

    /// <summary>
    /// Other charges such as late fees or reconnection fees.
    /// </summary>
    public decimal OtherCharges { get; private set; } // Late fees, reconnection fees, etc.

    /// <summary>
    /// Recorded kWh usage for the billing period.
    /// </summary>
    public decimal KWhUsed { get; private set; }

    /// <summary>
    /// Human-friendly billing period label (e.g., "2025-08").
    /// </summary>
    public string BillingPeriod { get; private set; }

    /// <summary>
    /// Date when the invoice was fully paid, if applicable.
    /// </summary>
    public DateTime? PaidDate { get; private set; }

    /// <summary>
    /// Payment method used for full payment, if applicable.
    /// </summary>
    public string? PaymentMethod { get; private set; }

    /// <summary>
    /// Optional late fee applied.
    /// </summary>
    public decimal? LateFee { get; private set; }

    /// <summary>
    /// Optional reconnection fee applied.
    /// </summary>
    public decimal? ReconnectionFee { get; private set; }

    /// <summary>
    /// Optional deposit amount included on the invoice.
    /// </summary>
    public decimal? DepositAmount { get; private set; }

    /// <summary>
    /// Rate schedule identifier/name applied to this invoice.
    /// </summary>
    public string? RateSchedule { get; private set; } // Rate schedule applied

    /// <summary>
    /// Optional demand charge component for C&I customers.
    /// </summary>
    public decimal? DemandCharge { get; private set; } // For commercial/industrial customers

    private readonly List<InvoiceLineItem> _lineItems = new();
    /// <summary>
    /// Additional line items with description, quantity, and unit price.
    /// </summary>
    public IReadOnlyCollection<InvoiceLineItem> LineItems => _lineItems.AsReadOnly();
    
    private Invoice()
    {
        // EF Core requires a parameterless constructor for entity instantiation
        InvoiceNumber = string.Empty;
        Status = string.Empty;
        BillingPeriod = string.Empty;
        PaidAmount = 0m;
    }

    private Invoice(string invoiceNumber, DefaultIdType memberId, DateTime invoiceDate,
        DateTime dueDate, DefaultIdType? ConsumptionId, decimal usageCharge, decimal basicServiceCharge,
        decimal taxAmount, decimal otherCharges, decimal kWhUsed, string billingPeriod,
        decimal? lateFee = null, decimal? reconnectionFee = null, decimal? depositAmount = null,
        string? rateSchedule = null, decimal? demandCharge = null, string? description = null, string? notes = null)
    {
        InvoiceNumber = invoiceNumber.Trim();
        Name = invoiceNumber.Trim(); // Keep for compatibility
        MemberId = memberId;
        InvoiceDate = invoiceDate;
        DueDate = dueDate;
        ConsumptionId = ConsumptionId;
        UsageCharge = usageCharge;
        BasicServiceCharge = basicServiceCharge;
        TaxAmount = taxAmount;
        OtherCharges = otherCharges;
        LateFee = lateFee;
        ReconnectionFee = reconnectionFee;
        DepositAmount = depositAmount;
        DemandCharge = demandCharge;
        TotalAmount = CalculateTotalAmount();
        PaidAmount = 0m;
        KWhUsed = kWhUsed;
        BillingPeriod = billingPeriod.Trim();
        RateSchedule = rateSchedule?.Trim();
        Status = "Draft";
        Description = description?.Trim();
        Notes = notes?.Trim();

        QueueDomainEvent(new InvoiceCreated(Id, InvoiceNumber, MemberId, TotalAmount, DueDate, Description, Notes));
    }

    /// <summary>
    /// Factory to create a new invoice with validation for required fields and non-negative charges.
    /// </summary>
    public static Invoice Create(string invoiceNumber, DefaultIdType memberId, DateTime invoiceDate,
        DateTime dueDate, DefaultIdType? ConsumptionId, decimal usageCharge, decimal basicServiceCharge,
        decimal taxAmount, decimal otherCharges, decimal kWhUsed, string billingPeriod,
        decimal? lateFee = null, decimal? reconnectionFee = null, decimal? depositAmount = null,
        string? rateSchedule = null, decimal? demandCharge = null, string? description = null, string? notes = null)
    {
        if (string.IsNullOrWhiteSpace(invoiceNumber))
            throw new ArgumentException("Invoice number cannot be empty");

        if (string.IsNullOrWhiteSpace(billingPeriod))
            throw new ArgumentException("Billing period cannot be empty");

        if (usageCharge < 0 || basicServiceCharge < 0 || taxAmount < 0 || otherCharges < 0)
            throw new ArgumentException("Charges cannot be negative");

        if (dueDate < invoiceDate)
            throw new ArgumentException("Due date cannot be before invoice date");

        return new Invoice(invoiceNumber, memberId, invoiceDate, dueDate,
            ConsumptionId, usageCharge, basicServiceCharge, taxAmount, otherCharges, kWhUsed,
            billingPeriod, lateFee, reconnectionFee, depositAmount, rateSchedule, demandCharge, description, notes);
    }

    /// <summary>
    /// Update invoice details and recompute total; disallowed when status is Paid.
    /// </summary>
    public Invoice Update(DateTime? dueDate = null, decimal? usageCharge = null, decimal? basicServiceCharge = null,
        decimal? taxAmount = null, decimal? otherCharges = null, decimal? lateFee = null,
        decimal? reconnectionFee = null, decimal? depositAmount = null, decimal? demandCharge = null,
        string? rateSchedule = null, string? description = null, string? notes = null)
    {
        if (Status == "Paid")
            throw new ArgumentException("Cannot modify paid invoice");

        bool isUpdated = false;

        if (dueDate.HasValue && DueDate != dueDate.Value)
        {
            if (dueDate.Value < InvoiceDate)
                throw new ArgumentException("Due date cannot be before invoice date");
            DueDate = dueDate.Value;
            isUpdated = true;
        }

        if (usageCharge.HasValue && UsageCharge != usageCharge.Value)
        {
            if (usageCharge.Value < 0)
                throw new ArgumentException("Usage charge cannot be negative");
            UsageCharge = usageCharge.Value;
            isUpdated = true;
        }

        if (basicServiceCharge.HasValue && BasicServiceCharge != basicServiceCharge.Value)
        {
            if (basicServiceCharge.Value < 0)
                throw new ArgumentException("Basic service charge cannot be negative");
            BasicServiceCharge = basicServiceCharge.Value;
            isUpdated = true;
        }

        if (taxAmount.HasValue && TaxAmount != taxAmount.Value)
        {
            if (taxAmount.Value < 0)
                throw new ArgumentException("Tax amount cannot be negative");
            TaxAmount = taxAmount.Value;
            isUpdated = true;
        }

        if (otherCharges.HasValue && OtherCharges != otherCharges.Value)
        {
            if (otherCharges.Value < 0)
                throw new ArgumentException("Other charges cannot be negative");
            OtherCharges = otherCharges.Value;
            isUpdated = true;
        }

        if (lateFee.HasValue && LateFee != lateFee.Value)
        {
            LateFee = lateFee.Value;
            isUpdated = true;
        }

        if (reconnectionFee.HasValue && ReconnectionFee != reconnectionFee.Value)
        {
            ReconnectionFee = reconnectionFee.Value;
            isUpdated = true;
        }

        if (depositAmount.HasValue && DepositAmount != depositAmount.Value)
        {
            DepositAmount = depositAmount.Value;
            isUpdated = true;
        }

        if (demandCharge.HasValue && DemandCharge != demandCharge.Value)
        {
            DemandCharge = demandCharge.Value;
            isUpdated = true;
        }

        if (!string.IsNullOrWhiteSpace(rateSchedule) && RateSchedule != rateSchedule.Trim())
        {
            RateSchedule = rateSchedule.Trim();
            isUpdated = true;
        }

        if (description != Description)
        {
            Description = description?.Trim();
            isUpdated = true;
        }

        if (notes != Notes)
        {
            Notes = notes?.Trim();
            isUpdated = true;
        }

        if (isUpdated)
        {
            TotalAmount = CalculateTotalAmount();
            QueueDomainEvent(new InvoiceUpdated(Id, InvoiceNumber, MemberId, TotalAmount, Description, Notes));
        }

        return this;
    }

    /// <summary>
    /// Set status to Sent; only allowed when in Draft.
    /// </summary>
    public Invoice Send()
    {
        if (Status != "Draft")
            throw new ArgumentException($"Cannot send invoice with status {Status}");

        Status = "Sent";
        QueueDomainEvent(new InvoiceSent(Id, InvoiceNumber, MemberId, TotalAmount, DueDate));
        return this;
    }

    /// <summary>
    /// Mark as fully paid, setting <see cref="PaidAmount"/>, <see cref="PaidDate"/>, and <see cref="PaymentMethod"/>.
    /// </summary>
    public Invoice MarkPaid(DateTime paidDate, string? paymentMethod = null)
    {
        if (Status == "Paid")
            throw new ArgumentException("Invoice is already paid");

        PaidAmount = TotalAmount;
        Status = "Paid";
        PaidDate = paidDate;
        PaymentMethod = paymentMethod?.Trim();

        QueueDomainEvent(new InvoicePaid(Id, InvoiceNumber, MemberId, TotalAmount, paidDate, paymentMethod));
        return this;
    }

    /// <summary>
    /// Apply a partial payment; when cumulative paid amount meets or exceeds total, marks as Paid.
    /// </summary>
    public Invoice ApplyPayment(decimal amount, DateTime paymentDate, string? paymentMethod = null)
    {
        if (amount <= 0) throw new ArgumentException("Payment amount must be positive");

        PaidAmount += amount;
        if (PaidAmount >= TotalAmount)
        {
            Status = "Paid";
            PaidDate = paymentDate;
            PaymentMethod = paymentMethod?.Trim();
            QueueDomainEvent(new InvoicePaid(Id, InvoiceNumber, MemberId, TotalAmount, paymentDate, paymentMethod));
        }
        else
        {
            QueueDomainEvent(new InvoicePartiallyPaid(Id, InvoiceNumber, MemberId, amount, PaidAmount));
        }

        return this;
    }

    /// <summary>
    /// Amount still due; never negative.
    /// </summary>
    public decimal GetOutstandingAmount() => Math.Max(0, TotalAmount - PaidAmount);

    /// <summary>
    /// Mark as Overdue when past due date and not yet paid.
    /// </summary>
    public Invoice MarkOverdue()
    {
        if (Status == "Paid")
            throw new ArgumentException("Cannot mark paid invoice as overdue");

        if (DateTime.Now > DueDate && Status != "Overdue")
        {
            Status = "Overdue";
            QueueDomainEvent(new InvoiceOverdue(Id, InvoiceNumber, MemberId, TotalAmount, DueDate));
        }

        return this;
    }

    /// <summary>
    /// Cancel an unpaid invoice.
    /// </summary>
    public Invoice Cancel(string? reason = null)
    {
        if (Status == "Paid")
            throw new ArgumentException("Cannot cancel paid invoice");

        Status = "Cancelled";
        QueueDomainEvent(new InvoiceCancelled(Id, InvoiceNumber, MemberId, reason));
        return this;
    }

    /// <summary>
    /// Append an additional line item to the invoice and update total.
    /// </summary>
    public Invoice AddLineItem(string description, decimal quantity, decimal unitPrice, string? accountCode = null)
    {
        if (Status == "Paid")
            throw new ArgumentException("Cannot modify paid invoice");

        var lineItem = InvoiceLineItem.Create(Id, description, quantity, unitPrice, accountCode);
        _lineItems.Add(lineItem);

        TotalAmount = CalculateTotalAmount();
        QueueDomainEvent(new InvoiceLineItemAdded(Id, InvoiceNumber, description, quantity * unitPrice));
        return this;
    }

    private decimal CalculateTotalAmount()
    {
        var lineItemsTotal = _lineItems.Sum(li => li.TotalPrice);
        return UsageCharge + BasicServiceCharge + TaxAmount + OtherCharges + 
               (LateFee ?? 0) + (ReconnectionFee ?? 0) + (DepositAmount ?? 0) + 
               (DemandCharge ?? 0) + lineItemsTotal;
    }
}

/// <summary>
/// A single line item on an invoice with quantity and pricing.
/// </summary>
public class InvoiceLineItem : BaseEntity
{
    /// <summary>
    /// Parent invoice identifier.
    /// </summary>
    public DefaultIdType InvoiceId { get; private set; }

    /// <summary>
    /// Description of the charge.
    /// </summary>
    public string Description { get; private set; }

    /// <summary>
    /// Quantity associated with this line item; must be positive.
    /// </summary>
    public decimal Quantity { get; private set; }

    /// <summary>
    /// Unit price to multiply by <see cref="Quantity"/>.
    /// </summary>
    public decimal UnitPrice { get; private set; }

    /// <summary>
    /// Calculated total price (Quantity * UnitPrice).
    /// </summary>
    public decimal TotalPrice { get; private set; }

    /// <summary>
    /// Optional revenue account code for this line.
    /// </summary>
    public string? AccountCode { get; private set; }

    private InvoiceLineItem(DefaultIdType invoiceId, string description, 
        decimal quantity, decimal unitPrice, string? accountCode = null)
    {
        InvoiceId = invoiceId;
        Description = description.Trim();
        Quantity = quantity;
        UnitPrice = unitPrice;
        TotalPrice = quantity * unitPrice;
        AccountCode = accountCode?.Trim();
    }

    /// <summary>
    /// Create a validated line item with positive quantity and non-negative unit price.
    /// </summary>
    public static InvoiceLineItem Create(DefaultIdType invoiceId, string description,
        decimal quantity, decimal unitPrice, string? accountCode = null)
    {
        if (string.IsNullOrWhiteSpace(description))
            throw new ArgumentException("Description cannot be empty");

        if (quantity <= 0)
            throw new ArgumentException("Quantity must be positive");

        if (unitPrice < 0)
            throw new ArgumentException("Unit price cannot be negative");

        return new InvoiceLineItem(invoiceId, description, quantity, unitPrice, accountCode);
    }
}

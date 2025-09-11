using System.ComponentModel.DataAnnotations.Schema;
using FSH.Framework.Core.Domain;
using FSH.Framework.Core.Domain.Contracts;

namespace FSH.Starter.WebApi.Warehouse.Domain;

public class Sale : AuditableEntity, IAggregateRoot
{
    public string SaleNumber { get; private set; } = string.Empty;
    public DateTime SaleDate { get; private set; }
    public SaleStatus Status { get; private set; } = SaleStatus.InProgress;
    [Column(TypeName = "decimal(18,2)")]
    public decimal SubTotal { get; private set; }
    [Column(TypeName = "decimal(18,2)")]
    public decimal TaxAmount { get; private set; }
    [Column(TypeName = "decimal(18,2)")]
    public decimal DiscountAmount { get; private set; }
    [Column(TypeName = "decimal(18,2)")]
    public decimal TotalAmount { get; private set; }
    [Column(TypeName = "decimal(18,2)")]
    public decimal AmountPaid { get; private set; }
    [Column(TypeName = "decimal(18,2)")]
    public decimal ChangeAmount { get; private set; }
    public string? CustomerName { get; private set; }
    public string? CustomerPhone { get; private set; }
    public string CashierName { get; private set; } = string.Empty;
    public string NotesText { get; private set; } = string.Empty;

    public DefaultIdType StoreId { get; private set; }
    public Store Store { get; private set; } = default!;
    public DefaultIdType? CustomerId { get; private set; }
    public Customer? Customer { get; private set; }

    public ICollection<SaleDetail> SaleDetails { get; private set; } = new List<SaleDetail>();
    public ICollection<Payment> Payments { get; private set; } = new List<Payment>();

    private Sale() { }

    public static Sale Create(string saleNumber, DateTime saleDate, decimal subTotal, decimal taxAmount, decimal discountAmount, decimal totalAmount, decimal amountPaid, decimal changeAmount, string? customerName, string? customerPhone, string cashierName, string? notes, DefaultIdType storeId, DefaultIdType? customerId)
    {
        return new Sale
        {
            SaleNumber = saleNumber,
            SaleDate = saleDate,
            SubTotal = subTotal,
            TaxAmount = taxAmount,
            DiscountAmount = discountAmount,
            TotalAmount = totalAmount,
            AmountPaid = amountPaid,
            ChangeAmount = changeAmount,
            CustomerName = customerName,
            CustomerPhone = customerPhone,
            CashierName = cashierName,
            NotesText = notes ?? string.Empty,
            StoreId = storeId,
            CustomerId = customerId
        };
    }

    public Sale Update(string? saleNumber, DateTime? saleDate, SaleStatus? status, decimal? subTotal, decimal? taxAmount, decimal? discountAmount, decimal? totalAmount, decimal? amountPaid, decimal? changeAmount, string? customerName, string? customerPhone, string? cashierName, string? notes)
    {
        if (!string.IsNullOrWhiteSpace(saleNumber)) SaleNumber = saleNumber;
        if (saleDate.HasValue) SaleDate = saleDate.Value;
        if (status.HasValue) Status = status.Value;
        if (subTotal.HasValue) SubTotal = subTotal.Value;
        if (taxAmount.HasValue) TaxAmount = taxAmount.Value;
        if (discountAmount.HasValue) DiscountAmount = discountAmount.Value;
        if (totalAmount.HasValue) TotalAmount = totalAmount.Value;
        if (amountPaid.HasValue) AmountPaid = amountPaid.Value;
        if (changeAmount.HasValue) ChangeAmount = changeAmount.Value;
        if (customerName is not null) CustomerName = customerName;
        if (customerPhone is not null) CustomerPhone = customerPhone;
        if (cashierName is not null) CashierName = cashierName;
        if (notes is not null) NotesText = notes;
        return this;
    }
}

public class SaleDetail : AuditableEntity
{
    public int Quantity { get; private set; }
    [Column(TypeName = "decimal(18,2)")]
    public decimal UnitPrice { get; private set; }
    [Column(TypeName = "decimal(18,2)")]
    public decimal LineTotal { get; private set; }
    [Column(TypeName = "decimal(18,2)")]
    public decimal DiscountAmount { get; private set; }

    public DefaultIdType SaleId { get; private set; }
    public Sale Sale { get; private set; } = default!;
    public DefaultIdType ProductId { get; private set; }
    public Product Product { get; private set; } = default!;
    public DefaultIdType? ProductBatchId { get; private set; }
    public ProductBatch? ProductBatch { get; private set; }

    private SaleDetail() { }

    public static SaleDetail Create(DefaultIdType saleId, DefaultIdType productId, int quantity, decimal unitPrice, decimal lineTotal, decimal discountAmount, DefaultIdType? productBatchId)
    {
        return new SaleDetail
        {
            SaleId = saleId,
            ProductId = productId,
            Quantity = quantity,
            UnitPrice = unitPrice,
            LineTotal = lineTotal,
            DiscountAmount = discountAmount,
            ProductBatchId = productBatchId
        };
    }

    public SaleDetail Update(int? quantity, decimal? unitPrice, decimal? lineTotal, decimal? discountAmount)
    {
        if (quantity.HasValue) Quantity = quantity.Value;
        if (unitPrice.HasValue) UnitPrice = unitPrice.Value;
        if (lineTotal.HasValue) LineTotal = lineTotal.Value;
        if (discountAmount.HasValue) DiscountAmount = discountAmount.Value;
        return this;
    }
}

public class Payment : AuditableEntity
{
    public PaymentMethod PaymentMethod { get; private set; }
    [Column(TypeName = "decimal(18,2)")]
    public decimal Amount { get; private set; }
    public string? Reference { get; private set; }
    public DateTime PaymentDate { get; private set; }

    public DefaultIdType SaleId { get; private set; }
    public Sale Sale { get; private set; } = default!;

    private Payment() { }

    public static Payment Create(DefaultIdType saleId, PaymentMethod method, decimal amount, string? reference, DateTime paymentDate)
    {
        return new Payment
        {
            SaleId = saleId,
            PaymentMethod = method,
            Amount = amount,
            Reference = reference,
            PaymentDate = paymentDate
        };
    }

    public Payment Update(PaymentMethod? method, decimal? amount, string? reference, DateTime? paymentDate)
    {
        if (method.HasValue) PaymentMethod = method.Value;
        if (amount.HasValue) Amount = amount.Value;
        if (reference is not null) Reference = reference;
        if (paymentDate.HasValue) PaymentDate = paymentDate.Value;
        return this;
    }
}


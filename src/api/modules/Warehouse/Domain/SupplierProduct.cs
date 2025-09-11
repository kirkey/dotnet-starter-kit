using System.ComponentModel.DataAnnotations.Schema;
using FSH.Framework.Core.Domain;

namespace FSH.Starter.WebApi.Warehouse.Domain;

public class SupplierProduct : AuditableEntity
{
    [Column(TypeName = "decimal(18,2)")]
    public decimal SupplierPrice { get; private set; }
    public string SupplierProductCode { get; private set; } = string.Empty;
    public int LeadTimeDays { get; private set; }
    public int MinimumOrderQuantity { get; private set; }
    public bool IsActive { get; private set; } = true;

    public DefaultIdType SupplierId { get; private set; }
    public Supplier Supplier { get; private set; } = default!;
    public DefaultIdType ProductId { get; private set; }
    public Product Product { get; private set; } = default!;

    private SupplierProduct() { }

    public static SupplierProduct Create(DefaultIdType supplierId, DefaultIdType productId, decimal supplierPrice, string supplierProductCode, int leadTimeDays, int minimumOrderQuantity, bool isActive = true)
    {
        return new SupplierProduct
        {
            SupplierId = supplierId,
            ProductId = productId,
            SupplierPrice = supplierPrice,
            SupplierProductCode = supplierProductCode,
            LeadTimeDays = leadTimeDays,
            MinimumOrderQuantity = minimumOrderQuantity,
            IsActive = isActive
        };
    }

    public SupplierProduct Update(decimal? supplierPrice, string? supplierProductCode, int? leadTimeDays, int? minimumOrderQuantity, bool? isActive)
    {
        if (supplierPrice.HasValue) SupplierPrice = supplierPrice.Value;
        if (supplierProductCode is not null) SupplierProductCode = supplierProductCode;
        if (leadTimeDays.HasValue) LeadTimeDays = leadTimeDays.Value;
        if (minimumOrderQuantity.HasValue) MinimumOrderQuantity = minimumOrderQuantity.Value;
        if (isActive.HasValue) IsActive = isActive.Value;
        return this;
    }
}


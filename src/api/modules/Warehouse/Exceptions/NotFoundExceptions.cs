using FSH.Framework.Core.Domain;
using FSH.Framework.Core.Exceptions;

namespace FSH.Starter.WebApi.Warehouse.Exceptions;

public sealed class CompanyNotFoundException(DefaultIdType id) : NotFoundException($"company with id {id} not found");
public sealed class StoreNotFoundException(DefaultIdType id) : NotFoundException($"store with id {id} not found");
public sealed class WarehouseNotFoundException(DefaultIdType id) : NotFoundException($"warehouse with id {id} not found");
public sealed class CategoryNotFoundException(DefaultIdType id) : NotFoundException($"category with id {id} not found");
public sealed class SupplierNotFoundException(DefaultIdType id) : NotFoundException($"supplier with id {id} not found");
public sealed class ProductNotFoundException(DefaultIdType id) : NotFoundException($"product with id {id} not found");
public sealed class PurchaseOrderNotFoundException(DefaultIdType id) : NotFoundException($"purchase order with id {id} not found");
public sealed class StoreTransferNotFoundException(DefaultIdType id) : NotFoundException($"store transfer with id {id} not found");
public sealed class SaleNotFoundException(DefaultIdType id) : NotFoundException($"sale with id {id} not found");
public sealed class CustomerNotFoundException(DefaultIdType id) : NotFoundException($"customer with id {id} not found");

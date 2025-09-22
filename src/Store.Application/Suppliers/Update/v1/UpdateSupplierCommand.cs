namespace FSH.Starter.WebApi.Store.Application.Suppliers.Update.v1;

/// <summary>
/// Command to update an existing Supplier. All properties are optional; only provided values will be updated.
/// </summary>
/// <param name="Id">The supplier identifier.</param>
/// <param name="Name">New supplier name. Max length 200.</param>
/// <param name="Description">New description. Max length 2000.</param>
/// <param name="ContactPerson">New primary contact. Max length 100.</param>
/// <param name="Email">New contact email. Max length 255.</param>
/// <param name="Phone">New contact phone. Max length 50.</param>
/// <param name="Address">New address. Max length 500.</param>
/// <param name="City">New city. Max length 100.</param>
/// <param name="State">New state/region. Max length 100.</param>
/// <param name="Country">New country. Max length 100.</param>
/// <param name="PostalCode">New postal code. Max length 20.</param>
/// <param name="Website">New website URL. Max length 255.</param>
/// <param name="CreditLimit">New credit limit. Must be &gt;= 0 if specified.</param>
/// <param name="PaymentTermsDays">New payment terms in days. Must be &gt;= 0.</param>
/// <param name="Rating">New rating between 0 and 5.</param>
/// <param name="Notes">New notes. Max length 2000.</param>
public sealed record UpdateSupplierCommand(
    DefaultIdType Id,
    string? Name,
    string? Description,
    string? ContactPerson,
    string? Email,
    string? Phone,
    string? Address,
    string? City,
    string? State,
    string? Country,
    string? PostalCode,
    string? Website,
    decimal? CreditLimit,
    int? PaymentTermsDays,
    decimal? Rating,
    string? Notes) : IRequest<UpdateSupplierResponse>;

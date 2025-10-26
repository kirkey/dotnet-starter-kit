namespace FSH.Starter.WebApi.Store.Application.Suppliers.Create.v1;

/// <summary>
/// Command to create a new Supplier aggregate with full details and business constraints.
/// </summary>
/// <param name="Name">The supplier name. Required. Max length 200.</param>
/// <param name="Description">Optional supplier description. Max length 2000.</param>
/// <param name="Code">Unique supplier code. Required. Max length 50.</param>
/// <param name="ContactPerson">Primary contact person. Required. Max length 100.</param>
/// <param name="Email">Contact email. Required. Max length 255.</param>
/// <param name="Phone">Contact phone. Required. Max length 50.</param>
/// <param name="Address">Supplier address. Required. Max length 500.</param>
/// <param name="PostalCode">Optional postal code. Max length 20.</param>
/// <param name="Website">Optional website URL. Max length 255.</param>
/// <param name="CreditLimit">Optional credit limit. Must be >= 0 if specified.</param>
/// <param name="PaymentTermsDays">Payment terms in days. Default 30. >= 0.</param>
/// <param name="IsActive">Whether the supplier is active. Default true.</param>
/// <param name="Rating">Initial rating between 0 and 5. Default 0.</param>
/// <param name="Notes">Optional notes up to 2000 characters.</param>
public sealed record CreateSupplierCommand(
    string Name,
    string? Description,
    string Code,
    string ContactPerson,
    string Email,
    string Phone,
    string Address,
    string? PostalCode = null,
    string? Website = null,
    decimal? CreditLimit = null,
    int PaymentTermsDays = 30,
    bool IsActive = true,
    decimal Rating = 0,
    string? Notes = null) : IRequest<CreateSupplierResponse>;

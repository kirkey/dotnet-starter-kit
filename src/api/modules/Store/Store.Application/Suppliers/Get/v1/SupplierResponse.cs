namespace FSH.Starter.WebApi.Store.Application.Suppliers.Get.v1;

/// <summary>
/// Data transfer object representing a supplier returned by queries.
/// </summary>
/// <param name="Id">Supplier identifier.</param>
/// <param name="Name">Supplier name.</param>
/// <param name="Description">Optional description.</param>
/// <param name="Code">Unique supplier code.</param>
/// <param name="ContactPerson">Primary contact person.</param>
/// <param name="Email">Contact email address.</param>
/// <param name="Phone">Contact phone number.</param>
/// <param name="Address">Address.</param>
/// <param name="PostalCode">Postal/ZIP code.</param>
/// <param name="Website">Website URL.</param>
/// <param name="CreditLimit">Optional credit limit.</param>
/// <param name="PaymentTermsDays">Payment terms in days.</param>
/// <param name="IsActive">Activation status.</param>
/// <param name="Rating">Rating 0..5.</param>
/// <param name="Notes">Optional notes.</param>
public sealed record SupplierResponse(
    DefaultIdType? Id,
    string Name,
    string? Description,
    string Code,
    string ContactPerson,
    string Email,
    string Phone,
    string Address,
    string? PostalCode,
    string? Website,
    decimal? CreditLimit,
    int PaymentTermsDays,
    bool IsActive,
    decimal Rating,
    string? Notes);

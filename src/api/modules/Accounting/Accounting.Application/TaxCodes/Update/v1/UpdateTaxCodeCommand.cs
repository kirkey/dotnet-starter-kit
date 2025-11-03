namespace Accounting.Application.TaxCodes.Update.v1;

/// <summary>
/// Command to update an existing tax code's non-rate information.
/// Rate updates should use a separate command to maintain rate history.
/// </summary>
public sealed record UpdateTaxCodeCommand(
    DefaultIdType Id,
    string? Name = null,
    string? Jurisdiction = null,
    string? TaxAuthority = null,
    string? TaxRegistrationNumber = null,
    string? ReportingCategory = null,
    string? Description = null) : IRequest<UpdateTaxCodeResponse>;


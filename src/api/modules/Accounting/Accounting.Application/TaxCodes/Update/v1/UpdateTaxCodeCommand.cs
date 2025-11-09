namespace Accounting.Application.TaxCodes.Update.v1;

/// <summary>
/// Command to update an existing tax code's non-rate information.
/// Rate updates should use a separate command to maintain rate history.
/// </summary>
public sealed record UpdateTaxCodeCommand : IRequest<UpdateTaxCodeResponse>
{
    /// <summary>
    /// The ID of the tax code to update.
    /// </summary>
    public DefaultIdType Id { get; init; }
    
    /// <summary>
    /// The name of the tax code.
    /// </summary>
    public string? Name { get; init; }
    
    /// <summary>
    /// The jurisdiction where the tax applies.
    /// </summary>
    public string? Jurisdiction { get; init; }
    
    /// <summary>
    /// The tax authority responsible for this tax.
    /// </summary>
    public string? TaxAuthority { get; init; }
    
    /// <summary>
    /// The tax registration number.
    /// </summary>
    public string? TaxRegistrationNumber { get; init; }
    
    /// <summary>
    /// The reporting category for tax filing.
    /// </summary>
    public string? ReportingCategory { get; init; }
    
    /// <summary>
    /// Description of the tax code.
    /// </summary>
    public string? Description { get; init; }
}

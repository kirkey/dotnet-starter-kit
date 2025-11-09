namespace Accounting.Application.DeferredRevenues.Update.v1;

/// <summary>
/// Command to update a deferred revenue.
/// </summary>
public sealed record UpdateDeferredRevenueCommand : IRequest<DefaultIdType>
{
    /// <summary>
    /// Deferred revenue identifier.
    /// </summary>
    public DefaultIdType Id { get; init; }
    
    /// <summary>
    /// Description.
    /// </summary>
    public string? Description { get; init; }
    
    /// <summary>
    /// Recognition date.
    /// </summary>
    public DateTime? RecognitionDate { get; init; }
}

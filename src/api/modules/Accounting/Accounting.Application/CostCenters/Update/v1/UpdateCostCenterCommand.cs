namespace Accounting.Application.CostCenters.Update.v1;

/// <summary>
/// Command to update a cost center.
/// </summary>
public sealed record UpdateCostCenterCommand : IRequest<DefaultIdType>
{
    /// <summary>
    /// Cost center identifier.
    /// </summary>
    public DefaultIdType Id { get; init; }
    
    /// <summary>
    /// Name.
    /// </summary>
    public string? Name { get; init; }
    
    /// <summary>
    /// Manager identifier.
    /// </summary>
    public DefaultIdType? ManagerId { get; init; }
    
    /// <summary>
    /// Manager name.
    /// </summary>
    public string? ManagerName { get; init; }
    
    /// <summary>
    /// Location.
    /// </summary>
    public string? Location { get; init; }
    
    /// <summary>
    /// End date.
    /// </summary>
    public DateTime? EndDate { get; init; }
    
    /// <summary>
    /// Description.
    /// </summary>
    public string? Description { get; init; }
    
    /// <summary>
    /// Notes.
    /// </summary>
    public string? Notes { get; init; }
}

namespace Accounting.Application.RetainedEarnings.RecordDistribution.v1;

/// <summary>
/// Command to record a distribution from retained earnings.
/// </summary>
public sealed record RecordDistributionCommand : IRequest<DefaultIdType>
{
    /// <summary>
    /// The ID of the retained earnings record.
    /// </summary>
    public DefaultIdType Id { get; init; }
    
    /// <summary>
    /// The distribution amount.
    /// </summary>
    public decimal Amount { get; init; }
    
    /// <summary>
    /// The date of the distribution.
    /// </summary>
    public DateTime DistributionDate { get; init; }
    
    /// <summary>
    /// The type/description of the distribution.
    /// </summary>
    public string? Description { get; init; }
    
    /// <summary>
    /// Additional notes about the distribution.
    /// </summary>
    public string? Notes { get; init; }
}

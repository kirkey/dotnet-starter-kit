namespace Accounting.Application.RetainedEarnings.RecordDistribution.v1;

public sealed record RecordDistributionCommand(
    DefaultIdType Id, 
    decimal Amount, 
    DateTime DistributionDate, 
    string DistributionType
) : IRequest<DefaultIdType>;


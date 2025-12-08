namespace FSH.Starter.WebApi.MicroFinance.Application.CollectionCases.RecordRecovery.v1;

public sealed record RecordCollectionCaseRecoveryResponse(
    DefaultIdType Id,
    decimal AmountRecovered,
    decimal AmountOverdue,
    string Status);

namespace FSH.Starter.WebApi.MicroFinance.Application.CollectionCases.RecordRecovery.v1;

public sealed record RecordCollectionCaseRecoveryResponse(
    Guid Id,
    decimal AmountRecovered,
    decimal AmountOverdue,
    string Status);

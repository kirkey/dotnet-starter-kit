namespace FSH.Starter.WebApi.MicroFinance.Application.LoanWriteOffs.RecordRecovery.v1;

public sealed record RecordWriteOffRecoveryResponse(
    Guid Id,
    decimal RecoveredAmount,
    decimal TotalWriteOff,
    decimal RemainingBalance);

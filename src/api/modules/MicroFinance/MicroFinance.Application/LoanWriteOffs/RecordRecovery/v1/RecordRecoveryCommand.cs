using MediatR;

namespace FSH.Starter.WebApi.MicroFinance.Application.LoanWriteOffs.RecordRecovery.v1;

public sealed record RecordWriteOffRecoveryCommand(
    DefaultIdType Id,
    decimal Amount,
    string? Notes = null) : IRequest<RecordWriteOffRecoveryResponse>;

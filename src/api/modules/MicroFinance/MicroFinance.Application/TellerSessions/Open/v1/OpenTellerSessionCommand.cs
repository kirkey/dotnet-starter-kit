using System.ComponentModel;
using MediatR;

namespace FSH.Starter.WebApi.MicroFinance.Application.TellerSessions.Open.v1;

public sealed record OpenTellerSessionCommand(
    [property: DefaultValue("00000000-0000-0000-0000-000000000000")] DefaultIdType BranchId,
    [property: DefaultValue("00000000-0000-0000-0000-000000000000")] DefaultIdType CashVaultId,
    [property: DefaultValue("TS-20241204-001")] string SessionNumber,
    [property: DefaultValue("00000000-0000-0000-0000-000000000000")] DefaultIdType TellerUserId,
    [property: DefaultValue("Juan Dela Cruz")] string TellerName,
    [property: DefaultValue(10000)] decimal OpeningBalance)
    : IRequest<OpenTellerSessionResponse>;

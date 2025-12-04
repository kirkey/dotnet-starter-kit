using System.ComponentModel;
using MediatR;

namespace FSH.Starter.WebApi.MicroFinance.Application.SavingsAccounts.Transfer.v1;

public sealed record TransferFundsCommand(
    [property: DefaultValue("00000000-0000-0000-0000-000000000000")] Guid FromAccountId,
    [property: DefaultValue("00000000-0000-0000-0000-000000000000")] Guid ToAccountId,
    [property: DefaultValue(1000)] decimal Amount,
    [property: DefaultValue("Internal transfer")] string? Notes) : IRequest<TransferFundsResponse>;

using MediatR;

namespace FSH.Starter.WebApi.MicroFinance.Application.ShareAccounts.PayDividend.v1;

/// <summary>
/// Command to pay out dividends from a share account.
/// </summary>
public sealed record PayDividendCommand(Guid ShareAccountId, decimal Amount) : IRequest<PayDividendResponse>;

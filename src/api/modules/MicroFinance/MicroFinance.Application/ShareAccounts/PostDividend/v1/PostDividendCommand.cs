using MediatR;

namespace FSH.Starter.WebApi.MicroFinance.Application.ShareAccounts.PostDividend.v1;

/// <summary>
/// Command to post dividend earnings to a share account.
/// </summary>
public sealed record PostDividendCommand(DefaultIdType ShareAccountId, decimal DividendAmount) : IRequest<PostDividendResponse>;

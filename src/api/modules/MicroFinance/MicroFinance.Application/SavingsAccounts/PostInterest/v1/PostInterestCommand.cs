using MediatR;

namespace FSH.Starter.WebApi.MicroFinance.Application.SavingsAccounts.PostInterest.v1;

/// <summary>
/// Command to post interest to a savings account.
/// </summary>
public sealed record PostInterestCommand(Guid AccountId, decimal InterestAmount) : IRequest<PostInterestResponse>;

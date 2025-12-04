using MediatR;

namespace FSH.Starter.WebApi.MicroFinance.Application.SavingsAccounts.Get.v1;

/// <summary>
/// Request to get a savings account by ID.
/// </summary>
public sealed record GetSavingsAccountRequest(Guid Id) : IRequest<SavingsAccountResponse>;

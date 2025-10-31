using Accounting.Application.InterCompanyTransactions.Responses;

namespace Accounting.Application.InterCompanyTransactions.Get;

/// <summary>
/// Request to get an inter-company transaction by ID.
/// </summary>
public record GetInterCompanyTransactionRequest(DefaultIdType Id) : IRequest<InterCompanyTransactionResponse>;


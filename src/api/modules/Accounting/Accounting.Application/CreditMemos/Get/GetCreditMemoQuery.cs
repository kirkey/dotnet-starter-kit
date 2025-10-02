using Accounting.Application.CreditMemos.Responses;

namespace Accounting.Application.CreditMemos.Get;

/// <summary>
/// Query to get a credit memo by ID.
/// </summary>
public sealed record GetCreditMemoQuery(DefaultIdType Id) : IRequest<CreditMemoResponse>;

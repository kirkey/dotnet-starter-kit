using Accounting.Application.DebitMemos.Responses;

namespace Accounting.Application.DebitMemos.Get;

/// <summary>
/// Query to get a debit memo by ID.
/// </summary>
public sealed record GetDebitMemoQuery(DefaultIdType Id) : IRequest<DebitMemoResponse>;

using MediatR;

namespace Accounting.Application.Currencies.Delete;

public record DeleteCurrencyRequest(DefaultIdType Id) : IRequest;

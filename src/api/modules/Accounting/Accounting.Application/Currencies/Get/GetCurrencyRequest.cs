using MediatR;
using Accounting.Application.Currencies.Dtos;

namespace Accounting.Application.Currencies.Get;

public record GetCurrencyRequest(DefaultIdType Id) : IRequest<CurrencyDto>;

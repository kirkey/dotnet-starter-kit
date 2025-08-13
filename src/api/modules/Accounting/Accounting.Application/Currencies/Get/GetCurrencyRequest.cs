using MediatR;
using Accounting.Application.Currencies.Dtos;

namespace Accounting.Application.Currencies.Get;

public class GetCurrencyRequest(DefaultIdType id) : IRequest<CurrencyDto>
{
    public DefaultIdType Id { get; set; } = id;
}

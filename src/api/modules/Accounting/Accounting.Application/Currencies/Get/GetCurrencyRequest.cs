using MediatR;
using Accounting.Application.Currencies.Dtos;

namespace Accounting.Application.Currencies.Get;

public class GetCurrencyRequest : IRequest<CurrencyDto>
{
    public DefaultIdType Id { get; set; }

    public GetCurrencyRequest(DefaultIdType id)
    {
        Id = id;
    }
}

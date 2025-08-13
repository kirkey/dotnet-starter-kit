using MediatR;

namespace Accounting.Application.Currencies.Delete;

public class DeleteCurrencyRequest : IRequest
{
    public DefaultIdType Id { get; set; }

    public DeleteCurrencyRequest(DefaultIdType id)
    {
        Id = id;
    }
}

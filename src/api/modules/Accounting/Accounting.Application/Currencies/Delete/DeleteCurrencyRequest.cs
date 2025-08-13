using MediatR;

namespace Accounting.Application.Currencies.Delete;

public class DeleteCurrencyRequest(DefaultIdType id) : IRequest
{
    public DefaultIdType Id { get; set; } = id;
}

using System.ComponentModel;
using MediatR;

namespace FSH.Starter.WebApi.Warehouse.Features.Stores.Create.v1;

public sealed record CreateStoreCommand(
    [property: DefaultValue("Downtown Store")] string Name,
    [property: DefaultValue("ST001")] string Code,
    [property: DefaultValue("500 High St")] string Address,
    [property: DefaultValue("+1234567890")] string Phone,
    [property: DefaultValue("Jane Manager")] string Manager,
    [property: DefaultValue(true)] bool IsActive,
    DefaultIdType CompanyId) : IRequest<CreateStoreResponse>;


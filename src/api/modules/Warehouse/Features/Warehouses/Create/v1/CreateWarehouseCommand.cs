using System.ComponentModel;
using MediatR;

namespace FSH.Starter.WebApi.Warehouse.Features.Warehouses.Create.v1;

public sealed record CreateWarehouseCommand(
    [property: DefaultValue("Main Warehouse")] string Name,
    [property: DefaultValue("WH001")] string Code,
    [property: DefaultValue("123 Market St")] string Address,
    [property: DefaultValue("+1234567890")] string Phone,
    [property: DefaultValue("John Manager")] string Manager,
    [property: DefaultValue(true)] bool IsActive,
    DefaultIdType CompanyId) : IRequest<CreateWarehouseResponse>;


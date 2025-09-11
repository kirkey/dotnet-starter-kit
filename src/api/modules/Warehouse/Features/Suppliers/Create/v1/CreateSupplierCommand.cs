// filepath: /Users/kirkeypsalms/Projects/dotnet-starter-kit/src/api/modules/Warehouse/Features/Suppliers/Create/v1/CreateSupplierCommand.cs
using System.ComponentModel;
using MediatR;

namespace FSH.Starter.WebApi.Warehouse.Features.Suppliers.Create.v1;

public sealed record CreateSupplierCommand(
    [property: DefaultValue("Acme Supplies")] string Name,
    [property: DefaultValue("SUP-ACME")] string Code,
    [property: DefaultValue("John Doe")] string ContactPerson,
    [property: DefaultValue("456 Industrial Rd")] string Address,
    [property: DefaultValue("+1987654321")] string Phone,
    [property: DefaultValue("supplies@acme.test")] string Email,
    [property: DefaultValue("TAX-987")] string TaxId,
    [property: DefaultValue(30)] int PaymentTermsDays,
    [property: DefaultValue(true)] bool IsActive) : IRequest<CreateSupplierResponse>;


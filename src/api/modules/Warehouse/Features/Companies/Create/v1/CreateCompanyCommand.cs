// filepath: /Users/kirkeypsalms/Projects/dotnet-starter-kit/src/api/modules/Warehouse/Features/Companies/Create/v1/CreateCompanyCommand.cs
using System.ComponentModel;
using MediatR;

namespace FSH.Starter.WebApi.Warehouse.Features.Companies.Create.v1;

public sealed record CreateCompanyCommand(
    [property: DefaultValue("Acme Corp")] string Name,
    [property: DefaultValue("123 Main St")] string Address,
    [property: DefaultValue("+1234567890")] string Phone,
    [property: DefaultValue("contact@acme.test")] string Email,
    [property: DefaultValue("TAX-123")] string TaxId) : IRequest<CreateCompanyResponse>;


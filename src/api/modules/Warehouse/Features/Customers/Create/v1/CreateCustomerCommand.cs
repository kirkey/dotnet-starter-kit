// filepath: /Users/kirkeypsalms/Projects/dotnet-starter-kit/src/api/modules/Warehouse/Features/Customers/Create/v1/CreateCustomerCommand.cs
using System.ComponentModel;
using MediatR;

namespace FSH.Starter.WebApi.Warehouse.Features.Customers.Create.v1;

public sealed record CreateCustomerCommand(
    [property: DefaultValue("Jane")] string FirstName,
    [property: DefaultValue("Doe")] string LastName,
    [property: DefaultValue("+1234567890")] string Phone,
    [property: DefaultValue("jane@example.com")] string Email,
    [property: DefaultValue("200 Main St")] string Address,
    DateTime? DateOfBirth) : IRequest<CreateCustomerResponse>;


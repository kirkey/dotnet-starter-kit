// filepath: /Users/kirkeypsalms/Projects/dotnet-starter-kit/src/api/modules/Warehouse/Features/Categories/Create/v1/CreateCategoryCommand.cs
using System.ComponentModel;
using MediatR;

namespace FSH.Starter.WebApi.Warehouse.Features.Categories.Create.v1;

public sealed record CreateCategoryCommand(
    [property: DefaultValue("Beverages")] string Name,
    [property: DefaultValue("CAT-BEV")] string Code,
    [property: DefaultValue("Drinks and beverages")] string? Description,
    [property: DefaultValue(true)] bool IsActive) : IRequest<CreateCategoryResponse>;


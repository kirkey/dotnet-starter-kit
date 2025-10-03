namespace FSH.Starter.WebApi.Store.Application.Bins.Create.v1;

public sealed record CreateBinCommand(
    [property: DefaultValue("Sample Bin")] string? Name,
    [property: DefaultValue("Primary storage bin")] string? Description = null,
    [property: DefaultValue("A1-01-01")] string? Code = null,
    DefaultIdType? WarehouseLocationId = null,
    [property: DefaultValue("Shelf")] string? BinType = "Shelf",
    [property: DefaultValue(null)] decimal? Capacity = null,
    [property: DefaultValue(0)] int Priority = 0
) : IRequest<CreateBinResponse>;

namespace FSH.Starter.WebApi.Store.Application.Bins.Update.v1;

public sealed record UpdateBinCommand(
    DefaultIdType Id,
    string? Name,
    string? Description,
    string? BinType,
    decimal? Capacity,
    int? Priority,
    bool? IsPickable,
    bool? IsPutable,
    string? Notes
) : IRequest<UpdateBinResponse>;

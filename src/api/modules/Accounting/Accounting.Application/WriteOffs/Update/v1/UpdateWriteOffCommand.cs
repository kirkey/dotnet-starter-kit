namespace Accounting.Application.WriteOffs.Update.v1;

public sealed record UpdateWriteOffCommand(
    DefaultIdType Id,
    string? Reason = null,
    string? Description = null,
    string? Notes = null
) : IRequest<DefaultIdType>;

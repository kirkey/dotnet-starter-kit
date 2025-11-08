namespace Accounting.Application.WriteOffs.Reverse.v1;

public sealed record ReverseWriteOffCommand(DefaultIdType Id, string? Reason = null) : IRequest<DefaultIdType>;


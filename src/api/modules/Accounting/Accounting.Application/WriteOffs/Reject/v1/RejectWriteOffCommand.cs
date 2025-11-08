namespace Accounting.Application.WriteOffs.Reject.v1;

public sealed record RejectWriteOffCommand(DefaultIdType Id, string RejectedBy, string? Reason = null) : IRequest<DefaultIdType>;


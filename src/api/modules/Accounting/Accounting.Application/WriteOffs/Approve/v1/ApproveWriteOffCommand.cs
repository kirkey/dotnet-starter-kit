namespace Accounting.Application.WriteOffs.Approve.v1;

public sealed record ApproveWriteOffCommand(DefaultIdType Id, string ApprovedBy) : IRequest<DefaultIdType>;


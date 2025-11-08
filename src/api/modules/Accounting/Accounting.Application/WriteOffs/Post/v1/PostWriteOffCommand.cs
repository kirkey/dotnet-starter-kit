namespace Accounting.Application.WriteOffs.Post.v1;

public sealed record PostWriteOffCommand(DefaultIdType Id, DefaultIdType JournalEntryId) : IRequest<DefaultIdType>;


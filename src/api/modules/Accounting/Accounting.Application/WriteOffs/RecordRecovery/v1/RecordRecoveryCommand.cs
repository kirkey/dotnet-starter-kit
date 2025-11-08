namespace Accounting.Application.WriteOffs.RecordRecovery.v1;

public sealed record RecordRecoveryCommand(
    DefaultIdType Id, 
    decimal RecoveryAmount, 
    DefaultIdType? RecoveryJournalEntryId = null
) : IRequest<DefaultIdType>;


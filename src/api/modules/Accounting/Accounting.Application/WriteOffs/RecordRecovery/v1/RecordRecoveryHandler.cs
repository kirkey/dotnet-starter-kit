namespace Accounting.Application.WriteOffs.RecordRecovery.v1;

public sealed class RecordRecoveryHandler(IRepository<WriteOff> repository, ILogger<RecordRecoveryHandler> logger)
    : IRequestHandler<RecordRecoveryCommand, DefaultIdType>
{
    private readonly IRepository<WriteOff> _repository = repository ?? throw new ArgumentNullException(nameof(repository));
    private readonly ILogger<RecordRecoveryHandler> _logger = logger ?? throw new ArgumentNullException(nameof(logger));

    public async Task<DefaultIdType> Handle(RecordRecoveryCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);
        _logger.LogInformation("Recording recovery for write-off {Id}: {Amount}", 
            request.Id, request.RecoveryAmount);

        var writeOff = await _repository.GetByIdAsync(request.Id, cancellationToken);
        if (writeOff == null) throw new NotFoundException($"Write-off with ID {request.Id} not found");

        writeOff.RecordRecovery(request.RecoveryAmount, request.RecoveryJournalEntryId);
        await _repository.UpdateAsync(writeOff, cancellationToken);
        await _repository.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Recovery recorded for {ReferenceNumber}: Total recovered={RecoveredAmount}", 
            writeOff.ReferenceNumber, writeOff.RecoveredAmount);
        return writeOff.Id;
    }
}


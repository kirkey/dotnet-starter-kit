namespace Accounting.Application.WriteOffs.Post.v1;

public sealed class PostWriteOffHandler(IRepository<WriteOff> repository, ILogger<PostWriteOffHandler> logger)
    : IRequestHandler<PostWriteOffCommand, DefaultIdType>
{
    private readonly IRepository<WriteOff> _repository = repository ?? throw new ArgumentNullException(nameof(repository));
    private readonly ILogger<PostWriteOffHandler> _logger = logger ?? throw new ArgumentNullException(nameof(logger));

    public async Task<DefaultIdType> Handle(PostWriteOffCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);
        _logger.LogInformation("Posting write-off {Id} to journal entry {JournalEntryId}", 
            request.Id, request.JournalEntryId);

        var writeOff = await _repository.GetByIdAsync(request.Id, cancellationToken);
        if (writeOff == null) throw new NotFoundException($"Write-off with ID {request.Id} not found");

        writeOff.Post(request.JournalEntryId);
        await _repository.UpdateAsync(writeOff, cancellationToken);
        await _repository.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Write-off {ReferenceNumber} posted to GL", writeOff.ReferenceNumber);
        return writeOff.Id;
    }
}


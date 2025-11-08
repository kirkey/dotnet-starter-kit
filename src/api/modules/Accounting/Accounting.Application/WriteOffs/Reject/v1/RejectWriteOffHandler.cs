namespace Accounting.Application.WriteOffs.Reject.v1;

public sealed class RejectWriteOffHandler(IRepository<WriteOff> repository, ILogger<RejectWriteOffHandler> logger)
    : IRequestHandler<RejectWriteOffCommand, DefaultIdType>
{
    private readonly IRepository<WriteOff> _repository = repository ?? throw new ArgumentNullException(nameof(repository));
    private readonly ILogger<RejectWriteOffHandler> _logger = logger ?? throw new ArgumentNullException(nameof(logger));

    public async Task<DefaultIdType> Handle(RejectWriteOffCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);
        _logger.LogInformation("Rejecting write-off {Id}", request.Id);

        var writeOff = await _repository.GetByIdAsync(request.Id, cancellationToken);
        if (writeOff == null) throw new NotFoundException($"Write-off with ID {request.Id} not found");

        writeOff.Reject(request.RejectedBy, request.Reason);
        await _repository.UpdateAsync(writeOff, cancellationToken);
        await _repository.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Write-off {ReferenceNumber} rejected by {RejectedBy}", 
            writeOff.ReferenceNumber, request.RejectedBy);
        return writeOff.Id;
    }
}


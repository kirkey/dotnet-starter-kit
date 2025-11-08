namespace Accounting.Application.WriteOffs.Approve.v1;

public sealed class ApproveWriteOffHandler(IRepository<WriteOff> repository, ILogger<ApproveWriteOffHandler> logger)
    : IRequestHandler<ApproveWriteOffCommand, DefaultIdType>
{
    private readonly IRepository<WriteOff> _repository = repository ?? throw new ArgumentNullException(nameof(repository));
    private readonly ILogger<ApproveWriteOffHandler> _logger = logger ?? throw new ArgumentNullException(nameof(logger));

    public async Task<DefaultIdType> Handle(ApproveWriteOffCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);
        _logger.LogInformation("Approving write-off {Id}", request.Id);

        var writeOff = await _repository.GetByIdAsync(request.Id, cancellationToken);
        if (writeOff == null) throw new NotFoundException($"Write-off with ID {request.Id} not found");

        writeOff.Approve(request.ApprovedBy);
        await _repository.UpdateAsync(writeOff, cancellationToken);
        await _repository.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Write-off {ReferenceNumber} approved by {ApprovedBy}", 
            writeOff.ReferenceNumber, request.ApprovedBy);
        return writeOff.Id;
    }
}


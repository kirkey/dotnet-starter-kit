namespace Accounting.Application.WriteOffs.Reverse.v1;

public sealed class ReverseWriteOffHandler(IRepository<WriteOff> repository, ILogger<ReverseWriteOffHandler> logger)
    : IRequestHandler<ReverseWriteOffCommand, DefaultIdType>
{
    private readonly IRepository<WriteOff> _repository = repository ?? throw new ArgumentNullException(nameof(repository));
    private readonly ILogger<ReverseWriteOffHandler> _logger = logger ?? throw new ArgumentNullException(nameof(logger));

    public async Task<DefaultIdType> Handle(ReverseWriteOffCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);
        _logger.LogInformation("Reversing write-off {Id}", request.Id);

        var writeOff = await _repository.GetByIdAsync(request.Id, cancellationToken);
        if (writeOff == null) throw new NotFoundException($"Write-off with ID {request.Id} not found");

        writeOff.Reverse(request.Reason);
        await _repository.UpdateAsync(writeOff, cancellationToken);
        await _repository.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Write-off {ReferenceNumber} reversed", writeOff.ReferenceNumber);
        return writeOff.Id;
    }
}

namespace Accounting.Application.WriteOffs.Update.v1;

public sealed class UpdateWriteOffHandler(IRepository<WriteOff> repository, ILogger<UpdateWriteOffHandler> logger)
    : IRequestHandler<UpdateWriteOffCommand, DefaultIdType>
{
    private readonly IRepository<WriteOff> _repository = repository ?? throw new ArgumentNullException(nameof(repository));
    private readonly ILogger<UpdateWriteOffHandler> _logger = logger ?? throw new ArgumentNullException(nameof(logger));

    public async Task<DefaultIdType> Handle(UpdateWriteOffCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);
        _logger.LogInformation("Updating write-off {Id}", request.Id);

        var writeOff = await _repository.GetByIdAsync(request.Id, cancellationToken);
        if (writeOff == null) throw new NotFoundException($"Write-off with ID {request.Id} not found");

        writeOff.Update(request.Reason, request.Description, request.Notes);
        await _repository.UpdateAsync(writeOff, cancellationToken);
        await _repository.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Write-off {ReferenceNumber} updated successfully", writeOff.ReferenceNumber);
        return writeOff.Id;
    }
}


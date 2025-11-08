namespace Accounting.Application.DepreciationMethods.Deactivate.v1;

public sealed class DeactivateDepreciationMethodHandler(
    IRepository<DepreciationMethod> repository,
    ILogger<DeactivateDepreciationMethodHandler> logger)
    : IRequestHandler<DeactivateDepreciationMethodCommand, DefaultIdType>
{
    private readonly IRepository<DepreciationMethod> _repository = repository ?? throw new ArgumentNullException(nameof(repository));
    private readonly ILogger<DeactivateDepreciationMethodHandler> _logger = logger ?? throw new ArgumentNullException(nameof(logger));

    public async Task<DefaultIdType> Handle(DeactivateDepreciationMethodCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);
        _logger.LogInformation("Deactivating depreciation method {Id}", request.Id);

        var method = await _repository.GetByIdAsync(request.Id, cancellationToken);
        if (method == null) throw new NotFoundException($"Depreciation method with ID {request.Id} not found");

        method.Deactivate();
        await _repository.UpdateAsync(method, cancellationToken);
        await _repository.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Depreciation method {MethodCode} deactivated successfully", method.MethodCode);
        return method.Id;
    }
}


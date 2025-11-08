namespace Accounting.Application.DepreciationMethods.Activate.v1;

public sealed class ActivateDepreciationMethodHandler(
    IRepository<DepreciationMethod> repository,
    ILogger<ActivateDepreciationMethodHandler> logger)
    : IRequestHandler<ActivateDepreciationMethodCommand, DefaultIdType>
{
    private readonly IRepository<DepreciationMethod> _repository = repository ?? throw new ArgumentNullException(nameof(repository));
    private readonly ILogger<ActivateDepreciationMethodHandler> _logger = logger ?? throw new ArgumentNullException(nameof(logger));

    public async Task<DefaultIdType> Handle(ActivateDepreciationMethodCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);
        _logger.LogInformation("Activating depreciation method {Id}", request.Id);

        var method = await _repository.GetByIdAsync(request.Id, cancellationToken);
        if (method == null) throw new NotFoundException($"Depreciation method with ID {request.Id} not found");

        method.Activate();
        await _repository.UpdateAsync(method, cancellationToken);
        await _repository.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Depreciation method {MethodCode} activated successfully", method.MethodCode);
        return method.Id;
    }
}


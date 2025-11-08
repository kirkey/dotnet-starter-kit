namespace Accounting.Application.FiscalPeriodCloses.Commands.v1;

/// <summary>
/// Handler for resolving a validation issue in the fiscal period close process.
/// </summary>
public sealed class ResolveFiscalPeriodCloseValidationIssueHandler(
    IRepository<FiscalPeriodClose> repository,
    ILogger<ResolveFiscalPeriodCloseValidationIssueHandler> logger)
    : IRequestHandler<ResolveFiscalPeriodCloseValidationIssueCommand, DefaultIdType>
{
    private readonly IRepository<FiscalPeriodClose> _repository = repository ?? throw new ArgumentNullException(nameof(repository));
    private readonly ILogger<ResolveFiscalPeriodCloseValidationIssueHandler> _logger = logger ?? throw new ArgumentNullException(nameof(logger));

    public async Task<DefaultIdType> Handle(ResolveFiscalPeriodCloseValidationIssueCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        _logger.LogInformation("Resolving validation issue for fiscal period close {CloseId}: {Issue}",
            request.FiscalPeriodCloseId, request.IssueDescription);

        var close = await _repository.GetByIdAsync(request.FiscalPeriodCloseId, cancellationToken);
        if (close == null)
        {
            _logger.LogWarning("Fiscal period close with ID {CloseId} not found", request.FiscalPeriodCloseId);
            throw new NotFoundException($"Fiscal period close with ID {request.FiscalPeriodCloseId} not found");
        }

        close.ResolveValidationIssue(request.IssueDescription, request.Resolution);

        await _repository.UpdateAsync(close, cancellationToken);
        await _repository.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Validation issue resolved for fiscal period close {CloseNumber}", close.CloseNumber);

        return close.Id;
    }
}

namespace Accounting.Application.FiscalPeriodCloses.Commands.v1;

/// <summary>
/// Handler for adding a validation issue to the fiscal period close process.
/// </summary>
public sealed class AddValidationIssueHandler(
    IRepository<FiscalPeriodClose> repository,
    ILogger<AddValidationIssueHandler> logger)
    : IRequestHandler<AddValidationIssueCommand, DefaultIdType>
{
    private readonly IRepository<FiscalPeriodClose> _repository = repository ?? throw new ArgumentNullException(nameof(repository));
    private readonly ILogger<AddValidationIssueHandler> _logger = logger ?? throw new ArgumentNullException(nameof(logger));

    public async Task<DefaultIdType> Handle(AddValidationIssueCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        _logger.LogInformation("Adding validation issue to fiscal period close {CloseId}: {Issue}",
            request.FiscalPeriodCloseId, request.IssueDescription);

        var close = await _repository.GetByIdAsync(request.FiscalPeriodCloseId, cancellationToken);
        if (close == null)
        {
            _logger.LogWarning("Fiscal period close with ID {CloseId} not found", request.FiscalPeriodCloseId);
            throw new NotFoundException($"Fiscal period close with ID {request.FiscalPeriodCloseId} not found");
        }

        close.AddValidationIssue(request.IssueDescription, request.Severity, request.Resolution);

        await _repository.UpdateAsync(close, cancellationToken);
        await _repository.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Validation issue added to fiscal period close {CloseNumber}", close.CloseNumber);

        return close.Id;
    }
}


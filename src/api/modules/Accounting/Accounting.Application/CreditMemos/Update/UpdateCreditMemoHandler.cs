using System.ComponentModel.DataAnnotations;

namespace Accounting.Application.CreditMemos.Update;

/// <summary>
/// Handler for updating a credit memo.
/// </summary>
/// <remarks>
/// Implements CQRS pattern. Performs strict validation on input fields and ensures robust error handling.
/// </remarks>
public sealed class UpdateCreditMemoHandler : IRequestHandler<UpdateCreditMemoCommand, DefaultIdType>
{
    private readonly ILogger<UpdateCreditMemoHandler> _logger;
    private readonly IRepository<CreditMemo> _repository;

    /// <summary>
    /// Initializes a new instance of the <see cref="UpdateCreditMemoHandler"/> class.
    /// </summary>
    /// <param name="logger">The logger instance.</param>
    /// <param name="repository">The credit memo repository.</param>
    public UpdateCreditMemoHandler(
        ILogger<UpdateCreditMemoHandler> logger,
        IRepository<CreditMemo> repository)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
    }

    /// <summary>
    /// Handles the update credit memo command.
    /// </summary>
    /// <param name="request">The update credit memo command.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The ID of the updated credit memo.</returns>
    /// <exception cref="CreditMemoNotFoundException">Thrown if the credit memo does not exist.</exception>
    /// <exception cref="CustomException">Thrown if the request contains invalid data.</exception>
    public async Task<DefaultIdType> Handle(UpdateCreditMemoCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        // Stricter validation for request fields
        if (string.IsNullOrWhiteSpace(request.Description))
            throw new CustomException("Description is required and cannot be empty.");
        if (request.Description.Length > 256)
            throw new CustomException("Description cannot exceed 256 characters.");
        if (!string.IsNullOrEmpty(request.Notes) && request.Notes.Length > 1024)
            throw new CustomException("Notes cannot exceed 1024 characters.");
        if (!string.IsNullOrEmpty(request.Reason) && request.Reason.Length > 128)
            throw new CustomException("Reason cannot exceed 128 characters.");

        var creditMemo = await _repository.GetByIdAsync(request.Id, cancellationToken);
        if (creditMemo == null)
        {
            _logger.LogWarning("Credit memo {CreditMemoId} not found for update", request.Id);
            throw new CreditMemoNotFoundException(request.Id);
        }

        creditMemo.Update(
            reason: request.Reason,
            description: request.Description,
            notes: request.Notes
        );

        await _repository.UpdateAsync(creditMemo, cancellationToken);
        await _repository.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Credit memo {CreditMemoId} updated", request.Id);

        return creditMemo.Id;
    }
}

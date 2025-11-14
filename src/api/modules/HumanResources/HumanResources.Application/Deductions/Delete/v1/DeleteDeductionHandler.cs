namespace FSH.Starter.WebApi.HumanResources.Application.Deductions.Delete.v1;

/// <summary>
/// Handler for deleting a deduction.
/// </summary>
public sealed class DeleteDeductionHandler(
    ILogger<DeleteDeductionHandler> logger,
    [FromKeyedServices("hr:deductions")] IRepository<PayComponent> repository)
    : IRequestHandler<DeleteDeductionCommand, DeleteDeductionResponse>
{
    public async Task<DeleteDeductionResponse> Handle(
        DeleteDeductionCommand request,
        CancellationToken cancellationToken)
    {
        var deduction = await repository.GetByIdAsync(request.Id, cancellationToken).ConfigureAwait(false);

        if (deduction is null)
            throw new Exception($"Deduction not found: {request.Id}");

        await repository.DeleteAsync(deduction, cancellationToken).ConfigureAwait(false);

        logger.LogInformation("Deduction {DeductionId} deleted successfully", deduction.Id);

        return new DeleteDeductionResponse(deduction.Id);
    }
}


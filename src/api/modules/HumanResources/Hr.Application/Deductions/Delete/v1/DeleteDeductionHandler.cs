namespace FSH.Starter.WebApi.HumanResources.Application.Deductions.Delete.v1;

/// <summary>
/// Handler for deleting a deduction.
/// </summary>
public sealed class DeleteDeductionHandler(
    ILogger<DeleteDeductionHandler> logger,
    [FromKeyedServices("hr:deductions")] IRepository<Deduction> repository)
    : IRequestHandler<DeleteDeductionCommand, DeleteDeductionResponse>
{
    public async Task<DeleteDeductionResponse> Handle(
        DeleteDeductionCommand request,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var deduction = await repository.GetByIdAsync(request.Id, cancellationToken).ConfigureAwait(false);

        if (deduction is null)
            throw new DeductionNotFoundException(request.Id);

        await repository.DeleteAsync(deduction, cancellationToken).ConfigureAwait(false);

        logger.LogInformation("Deduction {Id} deleted: {Name}", deduction.Id, deduction.DeductionName);

        return new DeleteDeductionResponse(deduction.Id, true);
    }
}

